
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public TMP_Text MingYuanText;
    public TMP_Text GoldText;
    public TMP_Text HealthText;
    public TMP_Text JingJieText;

    public Button MenuButton;

    public void Configure()
    {
        MenuButton.onClick.RemoveAllListeners();
        MenuButton.onClick.AddListener(OpenMenu);
    }

    public void Refresh()
    {
        IEntityModel entity = RunManager.Instance.Environment.Home;

        MingYuanText.text = RunManager.Instance.Environment.GetMingYuan().ToString();
        GoldText.text = RunManager.Instance.Environment.XiuWei.ToString();
        HealthText.text = entity.GetFinalHealth().ToString();
        JingJieText.text = $"{entity.GetJingJie().ToString()}期";
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }
}
