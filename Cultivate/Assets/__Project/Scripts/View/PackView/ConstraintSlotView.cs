
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConstraintSlotView : XView
{
    [SerializeField] public PackView PackView;

    protected override void AwakeFunction()
    {
        PackView.CheckAwake();
        base.AwakeFunction();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        PackView.SetAddress(GetAddress().Append(".Pack"));
    }

    public override void Refresh()
    {
        base.Refresh();

        PackConstraint constraint = Get<PackConstraint>();
        
        bool occupied = !constraint.IsEmpty;
        PackView.gameObject.SetActive(occupied);
        if (!occupied)
            return;
        
        PackView.Refresh();
        PackView.RefreshFromParentedConstraint(constraint);
    }

    private void OnEnable()
    {
        _interactBehaviour.LeftClickNeuron.Join(PackConstraintClicked);
        _interactBehaviour.PointerEnterNeuron.Join(HoverConstraint);
        _interactBehaviour.PointerExitNeuron.Join(UnhoverConstraint);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightConstraintsNeuron.Join(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightConstraintsNeuron.Join(Unhighlight);
    }

    private void OnDisable()
    {
        _interactBehaviour.LeftClickNeuron.Remove(PackConstraintClicked);
        _interactBehaviour.PointerEnterNeuron.Remove(HoverConstraint);
        _interactBehaviour.PointerExitNeuron.Remove(UnhoverConstraint);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightConstraintsNeuron.Remove(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightConstraintsNeuron.Remove(Unhighlight);
    }

    private void PackConstraintClicked(InteractBehaviour ib, PointerEventData data)
    {
        PackConstraintClickedDetails packConstraintClickedDetails = new(Get<PackConstraint>());
        AppManager.Instance.ConfigManager.PackTabControl.PackConstraintClickedProcedure(packConstraintClickedDetails);
    }

    private void HoverConstraint(InteractBehaviour ib, PointerEventData d)
    {
        PackConstraint constraint = Get<PackConstraint>();
        
        if (constraint.Pack != null)
        {
            // 存在已装备的卡牌，则高亮其Selection
            bool Pred(ConfigPack pack) => pack == constraint.Pack;
            CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightPacksNeuron.Invoke(Pred);
        }
        else
        {
            // 不存在已装备的卡牌，则高亮所有合法选择
            bool Pred(ConfigPack pack) => AppManager.Instance.ConfigManager.PackTabControl.IsCompatible(pack, constraint) && !pack.IsEquipped;
            CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightPacksNeuron.Invoke(Pred);
        }
    }

    private void UnhoverConstraint(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightPacksNeuron.Invoke();
    }

    private void Highlight(Predicate<PackConstraint> predicate)
    {
        if (!predicate(Get<PackConstraint>()))
            return;
        GetBehaviour<HighlightBehaviour>().SetHighlight(true);
    }

    private void Unhighlight()
    {
        GetBehaviour<HighlightBehaviour>().SetHighlight(false);
    }
}
