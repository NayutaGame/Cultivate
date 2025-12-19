
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConstraintSlotView : XView
{
    [SerializeField] private Image HoverImage;
    [SerializeField] public PackView PackView;

    protected override void AwakeFunction()
    {
        PackView.CheckAwake();
        base.AwakeFunction();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        PackView.SetAddress(GetAddress());
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
    }

    private void OnEnable()
    {
        if (GetBehaviour<ContentBehaviour>().Slot is CarrierSlotView carrier)
        {
            carrier.EnterIdleFunc = EnterIdleFunc;
            carrier.EnterHoverFunc = EnterHoverFunc;
        }
        _interactBehaviour.NeuronBundle.LeftClickNeuron.Join(PackConstraintClicked);
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Join(HoverConstraint);
        _interactBehaviour.NeuronBundle.PointerExitNeuron.Join(UnhoverConstraint);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightConstraintsNeuron.Join(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightConstraintsNeuron.Join(Unhighlight);
    }

    private void OnDisable()
    {
        if (GetBehaviour<ContentBehaviour>().Slot is CarrierSlotView carrier)
        {
            carrier.EnterIdleFunc = null;
            carrier.EnterHoverFunc = null;
        }
        _interactBehaviour.NeuronBundle.LeftClickNeuron.Remove(PackConstraintClicked);
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Remove(HoverConstraint);
        _interactBehaviour.NeuronBundle.PointerExitNeuron.Remove(UnhoverConstraint);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightConstraintsNeuron.Remove(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightConstraintsNeuron.Remove(Unhighlight);
    }

    private Tween EnterIdleFunc()
        => DOTween.Sequence()
            .Append(HoverImage.DOFade(0, 0.15f).SetEase(Ease.OutQuad))
            .Join(FollowAnimation.FromDefault(GetRect(), GetBehaviour<ContentBehaviour>().Slot.GetRect(), useSlotRotation: true).GetHandle());

    private Tween EnterHoverFunc()
        => DOTween.Sequence()
            .Append(HoverImage.DOFade(1, 0.15f).SetEase(Ease.OutQuad))
            .Join(FollowAnimation.FromDefault(GetRect(), GetBehaviour<ContentBehaviour>().Slot.GetRect(), useSlotRotation: true).GetHandle());

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
