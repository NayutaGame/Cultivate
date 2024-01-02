
public class SkillBrowserPanel : Panel
{
    public SkillInventoryView SkillInventoryView;

    public override void Configure()
    {
        base.Configure();
        SkillInventoryView.SetAddress(new Address("App.SkillInventory"));
        SkillInventoryView.PointerEnterNeuron.Set(CanvasManager.Instance.SkillAnnotation.SetAddressFromIB);
        SkillInventoryView.PointerExitNeuron.Set(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        SkillInventoryView.PointerMoveNeuron.Set(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
    }
}
