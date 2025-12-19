
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionPackView : PackView
{
    [SerializeField] private Image HoverImage;
    
    private void OnEnable()
    {
        if (GetBehaviour<ContentBehaviour>().Slot is CarrierSlotView carrier)
        {
            carrier.EnterIdleFunc = EnterIdleFunc;
            carrier.EnterHoverFunc = EnterHoverFunc;
        }
        _interactBehaviour.NeuronBundle.LeftClickNeuron.Join(PackSelectionClicked);
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Join(HoverSelection);
        _interactBehaviour.NeuronBundle.PointerExitNeuron.Join(UnhoverSelection);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightPacksNeuron.Join(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightPacksNeuron.Join(Unhighlight);
    }

    private void OnDisable()
    {
        if (GetBehaviour<ContentBehaviour>().Slot is CarrierSlotView carrier)
        {
            carrier.EnterIdleFunc = null;
            carrier.EnterHoverFunc = null;
        }
        _interactBehaviour.NeuronBundle.LeftClickNeuron.Remove(PackSelectionClicked);
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Remove(HoverSelection);
        _interactBehaviour.NeuronBundle.PointerExitNeuron.Remove(UnhoverSelection);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightPacksNeuron.Remove(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightPacksNeuron.Remove(Unhighlight);
    }

    private Tween EnterIdleFunc()
        => DOTween.Sequence()
            .Append(HoverImage.DOFade(0, 0.15f).SetEase(Ease.OutQuad))
            .Join(FollowAnimation.FromDefault(GetRect(), GetBehaviour<ContentBehaviour>().Slot.GetRect(), useSlotRotation: true).GetHandle());

    private Tween EnterHoverFunc()
        => DOTween.Sequence()
            .Append(HoverImage.DOFade(1, 0.15f).SetEase(Ease.OutQuad))
            .Join(FollowAnimation.FromDefault(GetRect(), GetBehaviour<ContentBehaviour>().Slot.GetRect(), useSlotRotation: true).GetHandle());

    private void PackSelectionClicked(InteractBehaviour ib, PointerEventData data)
    {
        PackSelectionClickedDetails packSelectionClickedDetails = new(Get<ConfigPack>());
        AppManager.Instance.ConfigManager.PackTabControl.PackSelectionClickedProcedure(packSelectionClickedDetails);
    }
    
    private void HoverSelection(InteractBehaviour ib, PointerEventData d)
    {
        ConfigPack pack = Get<ConfigPack>();

        if (pack.IsEquipped)
        {
            // 如果卡包已被装备，高亮其当前所在槽位
            bool Pred(PackConstraint constraint) => constraint.Pack == pack;
            CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightConstraintsNeuron.Invoke(Pred);
        }
        else
        {
            // 未装备时保持原有逻辑：高亮第一个可用槽位
            PackConstraint firstValidSlot = AppManager.Instance.ConfigManager.PackTabControl.GetFirstValidUnlockedSlot(pack);
            if (firstValidSlot == null) return;

            bool Pred(PackConstraint constraint) => constraint == firstValidSlot;
            CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightConstraintsNeuron.Invoke(Pred);
        }
    }

    private void UnhoverSelection(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightConstraintsNeuron.Invoke();
    }

    private void Highlight(Predicate<ConfigPack> predicate)
    {
        if (!predicate(Get<ConfigPack>()))
            return;
        GetBehaviour<HighlightBehaviour>().SetHighlight(true);
    }

    private void Unhighlight()
    {
        GetBehaviour<HighlightBehaviour>().SetHighlight(false);
    }
}