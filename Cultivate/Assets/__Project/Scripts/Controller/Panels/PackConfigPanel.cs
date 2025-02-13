
using System.Collections.Generic;
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

    private PackPreset _unmodifiedPackPreset;
    public void SetUnmodifiedPackPreset(PackPreset preset)
    {
        _unmodifiedPackPreset = preset;
    }
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        ConstraintListView.SetAddress(new Address("Config.PackConstraints"));
        ConstraintListView.LeftClickNeuron.Join(PackConstraintClicked);
        ConstraintListView.PointerEnterNeuron.Join(HoverConstraint);
        ConstraintListView.PointerExitNeuron.Join(UnhoverConstraint);

        SelectionListView.SetAddress(new Address("Config.PackSelections"));
        SelectionListView.LeftClickNeuron.Join(PackSelectionClicked);
        SelectionListView.PointerEnterNeuron.Join(HoverSelection);
        SelectionListView.PointerExitNeuron.Join(UnhoverSelection);
        
        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(Confirm);
        
        CancelButton.onClick.RemoveAllListeners();
        CancelButton.onClick.AddListener(Cancel);
    }

    public override void Refresh()
    {
        ConstraintListView.Refresh();
        SelectionListView.Refresh();
        RefreshConfirmButton();
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
        RefreshConfirmButton();
    }

    private void UnequipPackStaging(PackUnequipDetails d)
    {
        SelectionListView.Refresh();
        ConstraintListView.Refresh();
        RefreshConfirmButton();
    }

    private void RefreshConfirmButton()
    {
        ConfirmButton.interactable = AppManager.Instance.ConfigManager.IsConfigurationValid();
    }

    private async void Confirm()
    {
        // 保存选择的卡包
        _unmodifiedPackPreset = null;
        await GetAnimator().SetStateAsync(0);
    }
    
    private async void Cancel()
    {
        // 取消选择
        AppManager.Instance.ConfigManager.LoadPackPreset(_unmodifiedPackPreset);
        _unmodifiedPackPreset = null;
        await GetAnimator().SetStateAsync(0);
    }
    
    // public override void Refresh()
    // {
    //     PackListView.Refresh();
    // }
    
    public override void Return()
    {
        base.Return();
        Cancel();
    }

    private void HoverSelection(InteractBehaviour ib, PointerEventData d)
    {
        Debug.Log("HoverSelection");
        ConfigPack pack = ib.Get<ConfigPack>();
        ConfigManager configManager = AppManager.Instance.ConfigManager;

        PackConstraint firstValidSlot = configManager.GetFirstValidUnlockedSlot(pack);
        if (firstValidSlot == null)
            return;

        ConstraintListView.TraversalActive().Do(Highlight);
        
        void Highlight(XView view)
        {
            if (view.Get<PackConstraint>() == firstValidSlot)
            {
                view.GetBehaviour<HighlightBehaviour>().SetHighlight(true);
            }
        }
    }

    private void UnhoverSelection(InteractBehaviour ib, PointerEventData d)
    {
        ConstraintListView.TraversalActive().Do(Unhighlight);
        
        void Unhighlight(XView view)
        {
            view.GetBehaviour<HighlightBehaviour>().SetHighlight(false);
        }
    }

    private void HoverConstraint(InteractBehaviour ib, PointerEventData d)
    {
        PackConstraint constraint = ib.Get<PackConstraint>();
        ConfigManager configManager = AppManager.Instance.ConfigManager;

        List<ConfigPack> validPacks = new();
        
        if (constraint.Pack != null)
        {
            // 存在已装备的卡牌
            // 高亮已装备的卡牌
            validPacks.Add(constraint.Pack);
        }
        else
        {
            // 不存在已装备的卡牌
            // 高亮所有合法选择
            configManager.PackSelections.Traversal()
                .FilterObj(pack => configManager.IsCompatible(pack, constraint))
                .Do(pack => validPacks.Add(pack));
        }

        SelectionListView.TraversalActive().Do(Highlight);

        void Highlight(XView view)
        {
            ConfigPack pack = view.Get<ConfigPack>();
            if (validPacks.Contains(pack))
            {
                view.GetBehaviour<HighlightBehaviour>().SetHighlight(true);
            }
        }
    }

    private void UnhoverConstraint(InteractBehaviour ib, PointerEventData d)
    {
        SelectionListView.TraversalActive().Do(Unhighlight);
        
        void Unhighlight(XView view)
        {
            view.GetBehaviour<HighlightBehaviour>().SetHighlight(false);
        }
    }
}
