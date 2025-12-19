
using CLLibrary;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunConfigPanel : Panel
{
    [SerializeField] private CLButton ReturnButton;
    [SerializeField] private CLButton ProcessButton;
    [SerializeField] private TMP_Text ProcessButtonText;

    [SerializeField] private ListView TabList;

    [SerializeField] private GameObject[] PickerPanels;

    [SerializeField] private CharacterPickerPanel CharacterPickerPanel;
    [SerializeField] private DifficultyPickerPanel DifficultyPickerPanel;
    [SerializeField] public PackPickerPanel PackPickerPanel;
    
    [SerializeField] private RectTransform ModelAnchor;
    [SerializeField] private RectTransform[] AnchorList;

    private Tween _anchorHandle;
    
    // [SerializeField] private GameObject DemoLockedSign;

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
        PackPickerPanel.CheckAwake();
    }
    
    public override void Refresh()
    {
        int index = AppManager.Instance.ConfigManager.GetSelectedIndex();
        PickerPanels[index].SetActive(true);
        CheckValidation();

        // DemoLockedSign.SetActive(AppManager.Instance.PackageIsDemo() || AppManager.Instance.PackageIsForStream());
    }

    private void CheckValidation(CharacterSelectDetails d) => CheckValidation();
    private void CheckValidation(DifficultySelectDetails d) => CheckValidation();
    private void CheckValidation(PackEquipDetails d) => CheckValidation();
    private void CheckValidation(PackUnequipDetails d) => CheckValidation();

    private void CheckValidation()
    {
        bool isValid = AppManager.Instance.ConfigManager.IsValid();
        TabList.TraversalActive().Do(slotView =>
        {
            (slotView.GetContentView() as RunConfigTabView).ToggleButton.IsInteractable = isValid;
        });
        ProcessButton.SetStateToActiveIf(isValid);
    }
    
    private void OnEnable()
    {
        AppManager.Instance.ConfigManager.TabChangedNeuron.Add(TabChanged);

        AppManager.Instance.ConfigManager.CharacterTabControl.CharacterSelectNeuron.Add(CheckValidation);
        AppManager.Instance.ConfigManager.DifficultyTabControl.DifficultySelectNeuron.Add(CheckValidation);
        AppManager.Instance.ConfigManager.PackTabControl.EquipPackNeuron.Add(CheckValidation);
        AppManager.Instance.ConfigManager.PackTabControl.UnequipPackNeuron.Add(CheckValidation);
        
        ReturnButton.LeftClickNeuron.Add(Return);
        ProcessButton.LeftClickNeuron.Add(Process);
        
        Refresh();
        AppManager.Instance.PushEscFunc(Return);
    }
    
    private void OnDisable()
    {
        AppManager.Instance.ConfigManager.TabChangedNeuron.Remove(TabChanged);

        AppManager.Instance.ConfigManager.CharacterTabControl.CharacterSelectNeuron.Remove(CheckValidation);
        AppManager.Instance.ConfigManager.DifficultyTabControl.DifficultySelectNeuron.Remove(CheckValidation);
        AppManager.Instance.ConfigManager.PackTabControl.EquipPackNeuron.Remove(CheckValidation);
        AppManager.Instance.ConfigManager.PackTabControl.UnequipPackNeuron.Remove(CheckValidation);
        
        ReturnButton.LeftClickNeuron.Remove(Return);
        ProcessButton.LeftClickNeuron.Remove(Process);
        
        AppManager.Instance.PopEscFunc();
    }

    private void Process(InteractBehaviour ib, PointerEventData d)
        => AppManager.Instance.ConfigManager.ProcessProcedure();
    
    private void Return(InteractBehaviour ib, PointerEventData d)
        => Return();
    
    private void Return()
        => CloseRunConfigPanel();
    
    private async UniTask CloseRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(Panel.HIDE);
        await CanvasManager.Instance.AppCanvas.TitlePanel.GetAnimator().SetStateAsync(Panel.IDLE);
        AppManager.Instance.ConfigManager.ReadRecord();
    }

    private void TabChanged(RunConfigTabChangedDetails d)
    {
        SlotView fromSlot = TabList.ViewFromIndex(d.FromIndex);
        RunConfigTabView fromView = fromSlot.GetContentView() as RunConfigTabView;
        fromView.ToggleButton.IsDown = false;
        PickerPanels[d.FromIndex].SetActive(false);
        
        SlotView toSlot = TabList.ViewFromIndex(d.ToIndex);
        RunConfigTabView toView = toSlot.GetContentView() as RunConfigTabView;
        toView.ToggleButton.IsDown = true;
        PickerPanels[d.ToIndex].SetActive(true);
        
        _anchorHandle?.Kill();
        _anchorHandle = RigidAnimation.FromAbsolute(ModelAnchor, Configuration.FromRect(AnchorList[d.ToIndex])).GetHandle();
        _anchorHandle.SetAutoKill().Restart();
    }
    
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
    //     ConfirmButton.SetStateToActiveIf(AppManager.Instance.ConfigManager.IsConfigurationValid());
    // }
}
