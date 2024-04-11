
using UnityEngine.Serialization;

public class SkillShowcasePanel : Panel
{
    private Address _address;
    [FormerlySerializedAs("SkillInventoryView")] public ListView ListView;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("App.SkillShowcaseInventory");
        ListView.SetAddress(_address);
    }

    public override void Refresh()
    {
        base.Refresh();
        ListView.Refresh();
    }
}
