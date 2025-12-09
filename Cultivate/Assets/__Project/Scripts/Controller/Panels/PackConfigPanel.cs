
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackConfigPanel : PopupPanel
{
    [SerializeField] private GameObject BlockingCurtain;
    
    [SerializeField] private ListView ConstraintListView;
    [SerializeField] private ListView SelectionListView;
    [SerializeField] private CLButtonPatternA ConfirmButton;
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
    }

    private void OnEnable()
    {
        ConfirmButton.LeftClickNeuron.Add(Confirm);
        
        CancelButton.onClick.RemoveAllListeners();
        CancelButton.onClick.AddListener(Cancel);
        
        PackSelectionClickedEvent.Add(AppManager.Instance.ConfigManager.PackSelectionClickedProcedure);
        PackConstraintClickedEvent.Add(AppManager.Instance.ConfigManager.PackConstraintClickedProcedure);
        AppManager.Instance.ConfigManager.EquipPackNeuron.Add(EquipPackStaging);
        AppManager.Instance.ConfigManager.UnequipPackNeuron.Add(UnequipPackStaging);
        Refresh();
        
        ConstraintListView.ForceLayoutRebuild();
        ConstraintListView.RefreshPivots();
        
        SelectionListView.RefreshPivots();
        
        AppManager.Instance.PushEscFunc(Return);
    }

    private void OnDisable()
    {
        ConfirmButton.LeftClickNeuron.Remove(Confirm);
        
        CancelButton.onClick.RemoveAllListeners();
        
        PackSelectionClickedEvent.Remove(AppManager.Instance.ConfigManager.PackSelectionClickedProcedure);
        PackConstraintClickedEvent.Remove(AppManager.Instance.ConfigManager.PackConstraintClickedProcedure);
        AppManager.Instance.ConfigManager.EquipPackNeuron.Remove(EquipPackStaging);
        AppManager.Instance.ConfigManager.UnequipPackNeuron.Remove(UnequipPackStaging);
        
        AppManager.Instance.PopEscFunc();
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

    private void EquipPackStaging(PackEquipDetails d)
    {
        void SetPosition(SlotView view, XView otherView)
        {
            view.GetAnimator().SetState(3);
            view.GetContentView().GetRect().position = otherView.GetRect().position;
            view.GetContentView().GetRect().localScale = otherView.GetRect().localScale;
        }

        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(SlotView.IDLE);
            // AudioManager.Play("CardPlacement");
        }
        
        // ConstraintListView 设定动画
        SlotView constraintView = ConstraintListView.ViewFromIndex(d.ConstraintIndex);
        SlotView selectionView = SelectionListView.ViewFromIndex(d.SelectionIndex);
        
        constraintView.Refresh();
        selectionView.Refresh();
        
        SetPosition(constraintView, selectionView.GetContentView());
        constraintView.GetAnimator().SetStateAsync(SlotView.IDLE);
        selectionView.GetAnimator().SetState(SlotView.IDLE);
        
        // SelectionListView 归零 变暗
        
        RefreshConfirmButton();
    }

    private void UnequipPackStaging(PackUnequipDetails d)
    {
        void SetPosition(SlotView view, XView otherView)
        {
            view.GetAnimator().SetState(3);
            view.GetContentView().GetRect().position = otherView.GetRect().position;
            view.GetContentView().GetRect().localScale = otherView.GetRect().localScale;
        }

        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(SlotView.IDLE);
            // AudioManager.Play("CardPlacement");
        }
        
        // ConstraintListView 设定动画
        SlotView constraintView = ConstraintListView.ViewFromIndex(d.ConstraintIndex);
        SlotView selectionView = SelectionListView.ViewFromIndex(d.SelectionIndex);
        
        constraintView.Refresh();
        selectionView.Refresh();
        
        SetPosition(selectionView, constraintView.GetContentView());
        selectionView.GetAnimator().SetStateAsync(1);
        constraintView.GetAnimator().SetState(1);
        
        RefreshConfirmButton();
    }

    private void RefreshConfirmButton()
    {
        ConfirmButton.SetStateToActiveIf(AppManager.Instance.ConfigManager.IsConfigurationValid());
    }

    private void Confirm(InteractBehaviour ib, PointerEventData d)
    {
        // 保存选择的卡包
        _unmodifiedPackPreset = null;
        GetAnimator().SetStateAsync(0);
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
        Cancel();
    }

    private void HoverSelection(InteractBehaviour ib, PointerEventData d)
    {
        ConfigPack pack = ib.Get<ConfigPack>();
        ConfigManager configManager = AppManager.Instance.ConfigManager;

        if (pack.IsEquipped)
        {
            // 如果卡包已装备，高亮其当前所在槽位
            ConstraintListView.TraversalActive().Do(slotView =>
            {
                PackConstraint constraint = slotView.Get<PackConstraint>();
                if (constraint.Pack == pack)
                {
                    slotView.GetContentView().GetBehaviour<HighlightBehaviour>()?.SetHighlight(true);
                }
            });
        }
        else
        {
            // 未装备时保持原有逻辑：高亮第一个可用槽位
            PackConstraint firstValidSlot = configManager.GetFirstValidUnlockedSlot(pack);
            if (firstValidSlot == null) return;

            ConstraintListView.TraversalActive().Do(slotView =>
            {
                if (slotView.Get<PackConstraint>() == firstValidSlot)
                {
                    slotView.GetContentView().GetBehaviour<HighlightBehaviour>()?.SetHighlight(true);
                }
            });
        }
    }

    private void UnhoverSelection(InteractBehaviour ib, PointerEventData d)
    {
        ConstraintListView.TraversalActive().Do(Unhighlight);
        
        void Unhighlight(SlotView slotView)
        {
            slotView.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(false);
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
            configManager.PackSelections
            .FilterObj(pack => 
                configManager.IsCompatible(pack, constraint) && 
                !pack.IsEquipped)
                .Do(pack => validPacks.Add(pack));
        }

        SelectionListView.TraversalActive().Do(Highlight);

        void Highlight(SlotView slotView)
        {
            ConfigPack pack = slotView.Get<ConfigPack>();
            if (validPacks.Contains(pack))
            {
                slotView.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(true);
            }
        }
    }

    private void UnhoverConstraint(InteractBehaviour ib, PointerEventData d)
    {
        SelectionListView.TraversalActive().Do(Unhighlight);
        
        void Unhighlight(SlotView slotView)
        {
            slotView.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(false);
        }
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .Append(base.EnterIdle())
            .AppendCallback(() => BlockingCurtain.SetActive(false));

    public override Tween EnterHide()
        => DOTween.Sequence()
            .AppendCallback(() => BlockingCurtain.SetActive(true))
            .Append(base.EnterHide());
}
