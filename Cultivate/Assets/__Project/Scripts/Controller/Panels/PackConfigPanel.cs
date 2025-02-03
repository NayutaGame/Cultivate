
public class PackConfigPanel : PopupPanel
{
    public override void Return()
    {
        base.Return();

        GetAnimator().SetStateAsync(0);
    }
}
