
using CLLibrary;
using UnityEngine.UI;

public class SkillBrowserPanel : Panel
{
    private Address _address;

    public ListView SkillInventoryView;
    public Button[] SortButtons;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("App.SkillInventory");
        SkillInventoryView.SetAddress(_address);

        SortButtons.Length.Do(i =>
        {
            int comparisonId = i;
            SortButtons[i].onClick.RemoveAllListeners();
            SortButtons[i].onClick.AddListener(() => SortByComparisonId(comparisonId));
        });
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
    }

    private void SortByComparisonId(int i)
    {
        SkillInventory inventory = _address.Get<SkillInventory>();
        inventory.SortByComparisonId(i);
        Refresh();
    }
}
