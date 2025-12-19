
using System;
using UnityEngine.EventSystems;

public class SelectionPackView : PackView
{
    private void OnEnable()
    {
        _interactBehaviour.LeftClickNeuron.Join(PackSelectionClicked);
        _interactBehaviour.PointerEnterNeuron.Join(HoverSelection);
        _interactBehaviour.PointerExitNeuron.Join(UnhoverSelection);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightPacksNeuron.Join(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightPacksNeuron.Join(Unhighlight);
    }

    private void OnDisable()
    {
        _interactBehaviour.LeftClickNeuron.Remove(PackSelectionClicked);
        _interactBehaviour.PointerEnterNeuron.Remove(HoverSelection);
        _interactBehaviour.PointerExitNeuron.Remove(UnhoverSelection);
        
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.HighlightPacksNeuron.Remove(Highlight);
        CanvasManager.Instance.AppCanvas.RunConfigPanel.PackPickerPanel.UnhighlightPacksNeuron.Remove(Unhighlight);
    }

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