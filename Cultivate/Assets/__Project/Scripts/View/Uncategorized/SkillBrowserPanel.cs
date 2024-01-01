
using UnityEngine.EventSystems;

public class SkillBrowserPanel : Panel
{
    public SkillInventoryView SkillInventoryView;

    public override void Configure()
    {
        base.Configure();
        SkillInventoryView.SetAddress(new Address("App.SkillInventory"));
        SkillInventoryView.PointerEnterNeuron.Set(PointerEnter);
        SkillInventoryView.PointerExitNeuron.Set(PointerExit);
        SkillInventoryView.PointerMoveNeuron.Set(PointerMove);
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.SetAddressFromIB(ib, eventData);
    }

    private void PointerExit(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.SetAddressToNull(ib, eventData);
    }

    private void PointerMove(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(ib, eventData);
    }
}
