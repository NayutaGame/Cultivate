
using UnityEngine;
using UnityEngine.UI;

public class AchievementBrowserPanel : Panel
{
    private Address _address;

    public ListView AchievementInventoryView;

    [SerializeField] private Button ResetAchievementButton;
    [SerializeField] private Button ReturnButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("App.Profile.Curr.AchievementProfileList");
        AchievementInventoryView.SetAddress(_address);

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Hide);
        
        ResetAchievementButton.onClick.RemoveAllListeners();
        ResetAchievementButton.onClick.AddListener(ResetAchievement);
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

    public void ResetAchievement()
    {
        AppManager.Instance.ProfileManager.GetCurrProfile().ResetAchievementProfiles();
    }
    private void OnEnable()
    {
        AppManager.Instance.PushEscFunc(Hide);
    }

    private void OnDisable()
    {
        AppManager.Instance.PopEscFunc();
    }
}