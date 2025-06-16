
using UnityEngine;
using UnityEngine.UI;

public class AchievementBrowserPanel : Panel
{
    private Address _address;

    public ListView AchievementInventoryView;

    [SerializeField] private Button ReturnButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("App.AchievementList");
        AchievementInventoryView.SetAddress(_address);

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Hide);
    }

    public override void Refresh()
    {
        AchievementInventoryView.Refresh();
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