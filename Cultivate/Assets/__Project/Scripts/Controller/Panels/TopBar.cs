
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public ResourceView MingYuan;
    public ResourceView Gold;
    public ResourceView Health;
    
    public TMP_Text JingJieText;

    public Button MenuButton;

    public void Configure()
    {
        MenuButton.onClick.RemoveAllListeners();
        MenuButton.onClick.AddListener(OpenMenu);
        
        MingYuan.Configure(RunManager.Instance.Environment.GetMingYuan().ToString, RunManager.Instance.Environment.Home.MingYuan.GetMingYuanPenaltyText);
        // Gold.Configure(RunManager.Instance.Environment.Gold.ToString, () => "金钱");
        // Health.Configure(RunManager.Instance.Environment.Home.GetFinalHealth().ToString, () => "生命值上限");
    }

    private void OnEnable()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.Environment.ResourceChangedEvent += Refresh;
    }

    private void OnDisable()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.Environment.ResourceChangedEvent -= Refresh;
    }

    public void Refresh()
    {
        MingYuan.Refresh();
        // Gold.Refresh();
        // Health.Refresh();
        
        JingJieText.text = $"{RunManager.Instance.Environment.Home.GetJingJie().ToString()}期";
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }
}
