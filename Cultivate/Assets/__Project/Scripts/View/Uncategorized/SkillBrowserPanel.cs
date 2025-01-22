
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class SkillBrowserPanel : Panel
{
    private Address _address;

    public ListView SkillInventoryView;
    public Button[] SortButtons;

    [SerializeField] private Button ReturnButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("App.SkillInventory");
        SkillInventoryView.SetAddress(_address);

        SortButtons.Length.Do(i =>
        {
            int comparisonId = i;
            SortButtons[i].onClick.RemoveAllListeners();
            SortButtons[i].onClick.AddListener(() => SortByComparisonId(comparisonId));
        });

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Hide);
    }

    public override void Refresh()
    {
        SkillInventoryView.Refresh();
    }

    private void SortByComparisonId(int i)
    {
        SkillInventory inventory = _address.Get<SkillInventory>();
        inventory.SortByComparisonId(i);
        SkillInventoryView.Refresh();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
