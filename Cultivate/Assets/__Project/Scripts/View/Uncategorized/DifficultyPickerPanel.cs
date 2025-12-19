
using System;
using TMPro;
using UnityEngine;

public class DifficultyPickerPanel : Panel
{
    [SerializeField] private CurvedListView DifficultyListView;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private TMP_Text LockText;

    private Address _address;
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new("Config.DifficultyTabControl");
        
        DifficultyListView.SetAddress(new Address("Profile.ProfileList.Current.DifficultyProfileList"));
    }

    private void OnEnable()
    {
        AppManager.Instance.ConfigManager.DifficultyTabControl.DifficultySelectNeuron.Join(DifficultyChanged);

        DifficultyListView.Sync();
        RefreshCursor();
        Refresh();
    }

    private void OnDisable()
    {
        AppManager.Instance.ConfigManager.DifficultyTabControl.DifficultySelectNeuron.Remove(DifficultyChanged);
    }

    private void RefreshCursor()
    {
        int index = AppManager.Instance.ConfigManager.DifficultyTabControl.GetSelectedDifficultyIndex();
        DifficultyListView.SetCursor(index);
    }

    private void DifficultyChanged(DifficultySelectDetails d)
    {
        RunConfigDifficultyIconView fromView = DifficultyListView.ViewFromIndex(d.FromIndex).GetContentView() as RunConfigDifficultyIconView;
        RunConfigDifficultyIconView toView = DifficultyListView.ViewFromIndex(d.ToIndex).GetContentView() as RunConfigDifficultyIconView;

        fromView.SetSelected(false);
        toView.SetSelected(true);

        DifficultyListView.SetCursorAsync(d.ToIndex);

        Refresh();
    }
    
    public override void Refresh()
    {
        base.Refresh();

        DifficultyRunConfigTabControl control = _address.Get<DifficultyRunConfigTabControl>();
        DifficultyProfile difficultyProfile = control.GetSelectedDifficultyProfile();

        NameText.text = difficultyProfile.GetEntry().GetName();
        DescriptionText.text = difficultyProfile.GetEntry().Description;
        LockText.gameObject.SetActive(!difficultyProfile.IsUnlocked());
        LockText.text = "通关前一个难度之后解锁";
        
        // DemoMask.gameObject.SetActive(curr.IsDemoLocked());
    }
}
