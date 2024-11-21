
public class ArbitraryPickerSkillView : XView
{
    public XView SkillView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress());
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillView.Refresh();
    }
}
