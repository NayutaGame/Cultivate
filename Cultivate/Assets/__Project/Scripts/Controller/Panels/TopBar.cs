
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
        
        MingYuan.Configure(1, RunManager.Instance.Environment.GetMingYuan,
            RunManager.Instance.Environment.Home.MingYuan.GetMingYuanPenaltyText);
        Gold.Configure(1, RunManager.Instance.Environment.GetGold,
            () => "金钱");
        Health.Configure(1, RunManager.Instance.Environment.Home.GetFinalHealthBounded,
            () => "气血上限");
    }

    private void OnEnable()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanStagingNeuron.Add(GainMingYuan);
        CanvasManager.Instance.RunCanvas.LoseMingYuanStagingNeuron.Add(MingYuan.NumberChangeNoAnimation);
        
        CanvasManager.Instance.RunCanvas.GainGoldStagingNeuron.Add(GainGold);
        CanvasManager.Instance.RunCanvas.LoseGoldStagingNeuron.Add(Gold.NumberChange);
        
        CanvasManager.Instance.RunCanvas.GainDHealthStagingNeuron.Add(GainDHealth);
        CanvasManager.Instance.RunCanvas.LoseDHealthStagingNeuron.Add(Health.NumberChange);
    }

    private void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanStagingNeuron.Remove(GainMingYuan);
        CanvasManager.Instance.RunCanvas.LoseMingYuanStagingNeuron.Remove(MingYuan.NumberChangeNoAnimation);
        
        CanvasManager.Instance.RunCanvas.GainGoldStagingNeuron.Remove(GainGold);
        CanvasManager.Instance.RunCanvas.LoseGoldStagingNeuron.Remove(Gold.NumberChange);
        
        CanvasManager.Instance.RunCanvas.GainDHealthStagingNeuron.Remove(GainDHealth);
        CanvasManager.Instance.RunCanvas.LoseDHealthStagingNeuron.Remove(Health.NumberChange);
    }

    private void GainMingYuan(int value)
    {
        MingYuan.Gain(new Vector2(Screen.width / 2, Screen.height / 2), value);
    }

    private void GainGold(int value)
    {
        Gold.Gain(new Vector2(Screen.width / 2, Screen.height / 2), value);
    }

    private void GainDHealth(int value)
    {
        Health.Gain(new Vector2(Screen.width / 2, Screen.height / 2), value);
    }

    public void Refresh()
    {
        MingYuan.Refresh();
        Gold.Refresh();
        Health.Refresh();
        
        JingJieText.text = $"{RunManager.Instance.Environment.Home.GetJingJie().ToString()}期";
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }
}
