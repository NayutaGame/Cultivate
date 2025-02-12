
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackConfigPanel : PopupPanel
{
    [SerializeField] private AnimatedListView ConstraintListView;
    [SerializeField] private AnimatedListView SelectionListView;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button CancelButton;
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        ConstraintListView.SetAddress(new Address("Config.PackConstraints"));
        ConstraintListView.LeftClickNeuron.Join(PackConstraintClicked);

        SelectionListView.SetAddress(new Address("Config.PackSelections"));
        SelectionListView.LeftClickNeuron.Join(PackSelectionClicked);
        
        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(Confirm);
        
        CancelButton.onClick.RemoveAllListeners();
        CancelButton.onClick.AddListener(Cancel);
    }

    public override void Refresh()
    {
        ConstraintListView.Refresh();
        SelectionListView.Refresh();
    }

    public Neuron<PackSelectionClickedDetails> PackSelectionClickedEvent = new();
    public Neuron<PackConstraintClickedDetails> PackConstraintClickedEvent = new();

    private void PackSelectionClicked(InteractBehaviour ib, PointerEventData data)
    {
        PackSelectionClickedDetails packSelectionClickedDetails = new(ib.Get<ConfigPack>());
        PackSelectionClickedEvent.Invoke(packSelectionClickedDetails);
    }

    private void PackConstraintClicked(InteractBehaviour ib, PointerEventData data)
    {
        PackConstraintClickedDetails packConstraintClickedDetails = new(ib.Get<PackConstraint>());
        PackConstraintClickedEvent.Invoke(packConstraintClickedDetails);
    }

    private void OnEnable()
    {
        PackSelectionClickedEvent.Add(AppManager.Instance.ConfigManager.PackSelectionClickedProcedure);
        PackConstraintClickedEvent.Add(AppManager.Instance.ConfigManager.PackConstraintClickedProcedure);
        AppManager.Instance.ConfigManager.EquipPackNeuron.Add(EquipPackStaging);
        AppManager.Instance.ConfigManager.UnequipPackNeuron.Add(UnequipPackStaging);
        Refresh();
    }

    private void OnDisable()
    {
        PackSelectionClickedEvent.Remove(AppManager.Instance.ConfigManager.PackSelectionClickedProcedure);
        PackConstraintClickedEvent.Remove(AppManager.Instance.ConfigManager.PackConstraintClickedProcedure);
        AppManager.Instance.ConfigManager.EquipPackNeuron.Remove(EquipPackStaging);
        AppManager.Instance.ConfigManager.UnequipPackNeuron.Remove(UnequipPackStaging);
    }

    private void EquipPackStaging(PackEquipDetails d)
    {
        SelectionListView.Refresh();
        ConstraintListView.Refresh();
    }

    private void UnequipPackStaging(PackUnequipDetails d)
    {
        SelectionListView.Refresh();
        ConstraintListView.Refresh();
    }

    private async void Confirm()
    {
        // 保存选择的卡包
        await GetAnimator().SetStateAsync(0);
    }
    
    private async void Cancel()
    {
        // 取消选择
        await GetAnimator().SetStateAsync(0);
    }
    
    // public override void Refresh()
    // {
    //     PackListView.Refresh();
    // }
    
    public override void Return()
    {
        base.Return();
        GetAnimator().SetStateAsync(0);
    }
}
