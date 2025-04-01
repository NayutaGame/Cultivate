
using System;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class DetailedCharacterProfileView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text AbilityDescriptionText;
    [SerializeField] private TMP_Text UnlockConditionText;
    [SerializeField] private RectTransform Anchor;
    [NonSerialized] private PrefabEntry PrefabEntry;
    [NonSerialized] private GameObject Model;

    public override void Refresh()
    {
        base.Refresh();

        if (GetAddress() == null)
        {
            NameText.text = "请选择一名角色";
            AbilityDescriptionText.text = "角色的能力";
            UnlockConditionText.gameObject.SetActive(false);
            SetPrefabEntry(null);
            return;
        }

        CharacterProfile p = Get<CharacterProfile>();
        NameText.text = p.GetEntry().GetName();
        AbilityDescriptionText.text = p.GetEntry().AbilityDescription;

        UnlockConditionText.gameObject.SetActive(!p.IsUnlocked());

        var lockIndex = p.ToCharacterLockIndex();
        var achievementProfile = AppManager.Instance.ProfileManager.GetCurrProfile().GetAchievementProfileFromLockIndex(lockIndex);
        if (achievementProfile != null)
        {
            UnlockConditionText.text = achievementProfile.GetEntry().GetConditionDescription();
        }
        else
        {
            UnlockConditionText.text = "没有对应的解锁方法";
        }
        SetPrefabEntry(p.GetEntry().GetConfigPrefabEntry());
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

        // AwayModel = AwayGameObject.GetComponent<IStageModel>();
        // AwayModel.BaseTransform = AwayAnchor;
    }
}
