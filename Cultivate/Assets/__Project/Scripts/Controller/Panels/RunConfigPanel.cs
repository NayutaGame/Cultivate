
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunConfigPanel : Panel
{
    [SerializeField] private CLButtonPatternA ReturnButton;
    [SerializeField] private CLButtonPatternA ProcessButton;

    [SerializeField] private ListView TabList;

    [SerializeField] private GameObject[] PickerPanels;

    [SerializeField] private CharacterPickerPanel CharacterPickerPanel;
    [SerializeField] private DifficultyPickerPanel DifficultyPickerPanel;
    // [SerializeField] private PackPickerPanel PackPickerPanel;
    
    // [SerializeField] private GameObject DemoLockedSign;
    // [SerializeField] private CLButtonPatternA PackConfigButton;
    // [SerializeField] private PackConfigPanel PackConfigPanel;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        TabList.SetAddress("Config.RunConfigTabControls");
        TabList.SetPrefabProvider(model =>
        {
            if (model is CharacterRunConfigTabControl)
                return 0;
            if (model is DifficultyRunConfigTabControl)
                return 1;
            if (model is PackRunConfigTabControl)
                return 2;
            return -1;
        });
        
        CharacterPickerPanel.CheckAwake();
        DifficultyPickerPanel.CheckAwake();

        // PackConfigPanel.CheckAwake();
        //
        // DifficultyPickerView.Configure();
    }
    
    public override void Refresh()
    {
        int index = AppManager.Instance.ConfigManager.GetSelectedIndex();
        if (index < PickerPanels.Length)
            PickerPanels[index].SetActive(true);

        // DemoLockedSign.SetActive(AppManager.Instance.PackageIsDemo() || AppManager.Instance.PackageIsForStream());
        // RefreshAllSelection();
    }
    
    private void OnEnable()
    {
        AppManager.Instance.ConfigManager.TabChangedNeuron.Add(TabChanged);
        
        ReturnButton.LeftClickNeuron.Add(Return);
        ReturnButton.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
        ReturnButton.GetInteractBehaviour().PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        // StartRunButton.LeftClickNeuron.Add(StartRun);
        // StartRunButton.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
        // StartRunButton.GetInteractBehaviour().PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        // PackConfigButton.LeftClickNeuron.Add(EnterPackConfig);
        // PackConfigButton.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
        // PackConfigButton.GetInteractBehaviour().PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        
        Refresh();
        AppManager.Instance.PushEscFunc(Return);
    }
    
    private void OnDisable()
    {
        AppManager.Instance.ConfigManager.TabChangedNeuron.Remove(TabChanged);
        
        ReturnButton.LeftClickNeuron.Remove(Return);
        ReturnButton.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        ReturnButton.GetInteractBehaviour().PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        // StartRunButton.LeftClickNeuron.Remove(StartRun);
        // StartRunButton.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        // StartRunButton.GetInteractBehaviour().PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        // PackConfigButton.LeftClickNeuron.Remove(EnterPackConfig);
        // PackConfigButton.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        // PackConfigButton.GetInteractBehaviour().PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        
        AppManager.Instance.PopEscFunc();
    }
    
    private void Return(InteractBehaviour ib, PointerEventData d)
        => Return();
    
    private void Return()
    {
        CloseRunConfigPanel();
    }
    
    private async UniTask CloseRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(Panel.HIDE);
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(Panel.IDLE);
    }

    private void TabChanged(RunConfigTabChangedDetails d)
    {
        SlotView fromSlot = TabList.ViewFromIndex(d.FromIndex);
        RunConfigTabView fromView = fromSlot.GetContentView() as RunConfigTabView;
        fromView.ToggleButton.IsDown = false;
        if (d.FromIndex < PickerPanels.Length)
            PickerPanels[d.FromIndex].SetActive(false);
        
        SlotView toSlot = TabList.ViewFromIndex(d.ToIndex);
        RunConfigTabView toView = toSlot.GetContentView() as RunConfigTabView;
        toView.ToggleButton.IsDown = true;
        if (d.ToIndex < PickerPanels.Length)
            PickerPanels[d.ToIndex].SetActive(true);
    }
    
    // private void EnterPackConfig(InteractBehaviour ib, PointerEventData d)
    // {
    //     PackConfigPanel.SetUnmodifiedPackPreset(AppManager.Instance.ConfigManager.WriteCurrentIntoPackPreset());
    //     PackConfigPanel.GetAnimator().SetStateAsync(1);
    // }
    //
    //
    // private void StartRun(InteractBehaviour ib, PointerEventData d)
    // {
    //     CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;
    //     List<PackEntry> packEntries = AppManager.Instance.ConfigManager.GetEquippedPacks();
    //     RunConfig runConfig = new(characterProfile, DifficultyPickerView.GetSelection(), packEntries);
    //     AppManager.Instance.Push(AppStateMachine.RUN, runConfig);
    // }
    //
    // private void RefreshAllSelection()
    // {
    //     SelectBehaviour currentCharacterSelectBehaviour = GetCurrentCharacterSelectBehaviour();
    //     foreach (var slotView in CharacterListView.Traversal())
    //     {
    //         SelectBehaviour s = slotView.GetContentView().GetBehaviour<SelectBehaviour>();
    //         s.SetSelect(s == currentCharacterSelectBehaviour);
    //     }
    //
    //     DetailedCharacterProfileView.SetAddress(currentCharacterSelectBehaviour.GetAddress());
    //     DetailedCharacterProfileView.Refresh();
    //
    //     RefreshStartRunButton();
    // }
    //
    // public void RefreshStartRunButton()
    // {
    //     CharacterProfile characterProfile = AppManager.Instance.ConfigManager.SelectedCharacter;
    //
    //     bool interactable = characterProfile.IsUnlocked() &&
    //                         !characterProfile.IsDemoLocked() &&
    //                         !characterProfile.IsForStreamLocked() &&
    //                         !DifficultyPickerView.GetSelection().IsDemoLocked() &&
    //                         DifficultyPickerView.GetSelection().IsUnlocked();
    //     
    //     StartRunButton.SetStateToActiveIf(interactable);
    // }
}
