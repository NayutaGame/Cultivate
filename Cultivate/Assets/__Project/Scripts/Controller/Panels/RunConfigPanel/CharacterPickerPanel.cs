
using System;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class CharacterPickerPanel : Panel
{
    [SerializeField] private CurvedListView CharacterListView;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private TMP_Text LockText;
    
    [SerializeField] private RectTransform Anchor;
    [NonSerialized] private PrefabEntry PrefabEntry;
    [NonSerialized] private GameObject Model;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new("Config.CharacterTabControl");
        
        CharacterListView.SetAddress(new Address("Profile.ProfileList.Current.CharacterProfileList"));
    }

    private void OnEnable()
    {
        AppManager.Instance.ConfigManager.CharacterTabControl.CharacterSelectNeuron.Add(CharacterChanged);
        
        RefreshCursor();
        Refresh();
    }

    private void OnDisable()
    {
        AppManager.Instance.ConfigManager.CharacterTabControl.CharacterSelectNeuron.Remove(CharacterChanged);
    }

    private void RefreshCursor()
    {
        int index = AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterIndex();
        CharacterListView.SetCursor(index);
    }

    private void CharacterChanged(CharacterSelectDetails d)
    {
        RunConfigCharacterIconView fromView = CharacterListView.ViewFromIndex(d.FromIndex).GetContentView() as RunConfigCharacterIconView;
        RunConfigCharacterIconView toView = CharacterListView.ViewFromIndex(d.ToIndex).GetContentView() as RunConfigCharacterIconView;

        fromView.SetSelected(false);
        toView.SetSelected(true);

        CharacterListView.SetCursorAsync(d.ToIndex);

        Refresh();
    }
    
    public override void Refresh()
    {
        base.Refresh();

        CharacterRunConfigTabControl control = _address.Get<CharacterRunConfigTabControl>();
        CharacterProfile characterProfile = control.GetSelectedCharacterProfile();
        if (characterProfile == null)
        {
            NameText.text = "请选择一名角色";
            DescriptionText.text = "角色的能力";
            LockText.gameObject.SetActive(false);
            SetPrefabEntry(null);
            return;
        }

        NameText.text = characterProfile.GetEntry().GetName();
        DescriptionText.text = characterProfile.GetEntry().GetAbilityDescription().GetHighlightedString();

        LockText.gameObject.SetActive(!characterProfile.IsUnlocked());

        var lockIndex = characterProfile.ToCharacterLockIndex();
        var achievementProfile = AppManager.Instance.ProfileManager.GetCurrProfile().GetAchievementProfileFromLockIndex(lockIndex);
        if (achievementProfile != null)
        {
            LockText.text = achievementProfile.GetEntry().GetConditionDescription().GetHighlightedString();
        }
        else
        {
            LockText.text = "没有对应的解锁方法";
        }
        SetPrefabEntry(characterProfile.GetEntry().GetConfigPrefabEntry());
    }

    private void SetPrefabEntry(PrefabEntry prefabEntry)
    {
        if (PrefabEntry == prefabEntry)
            return;
        
        if (Model != null)
            Destroy(Model);

        PrefabEntry = prefabEntry;
        Model = Instantiate(prefabEntry.Prefab, Anchor);

        SkeletonGraphic skeletonGraphic = Model.GetComponentInChildren<SkeletonGraphic>();
        if (skeletonGraphic != null)
        {
            skeletonGraphic.AnimationState.SetAnimation(1, "win", false);
            skeletonGraphic.AnimationState.AddAnimation(1, "idle", true, 0);
        }
    }
}