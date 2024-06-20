
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
            () => "生命值上限");
    }

    private void OnEnable()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanStagingNeuron.Add(GainMingYuan);
        CanvasManager.Instance.RunCanvas.LoseMingYuanStagingNeuron.Add(LoseMingYuan);
        
        CanvasManager.Instance.RunCanvas.GainGoldStagingNeuron.Add(GainGold);
        CanvasManager.Instance.RunCanvas.ConsumeGoldStagingNeuron.Add(ConsumeGold);
        
        CanvasManager.Instance.RunCanvas.GainDHealthStagingNeuron.Add(GainDHealth);
        CanvasManager.Instance.RunCanvas.LoseDHealthStagingNeuron.Add(LoseDHealth);
    }

    private void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanStagingNeuron.Remove(GainMingYuan);
        CanvasManager.Instance.RunCanvas.LoseMingYuanStagingNeuron.Remove(LoseMingYuan);
        
        CanvasManager.Instance.RunCanvas.GainGoldStagingNeuron.Remove(GainGold);
        CanvasManager.Instance.RunCanvas.ConsumeGoldStagingNeuron.Remove(ConsumeGold);
        
        CanvasManager.Instance.RunCanvas.GainDHealthStagingNeuron.Remove(GainDHealth);
        CanvasManager.Instance.RunCanvas.LoseDHealthStagingNeuron.Remove(LoseDHealth);
    }

    private void GainMingYuan(SetDMingYuanDetails d)
    {
        MingYuan.Gain(new Vector2(Screen.width / 2, Screen.height / 2), d.Value);
    }

    private void LoseMingYuan(SetDMingYuanDetails d)
    {
        MingYuan.NumberChangeNoAnimation(d.Value);
    }

    private void GainGold(SetDGoldDetails d)
    {
        Gold.Gain(new Vector2(Screen.width / 2, Screen.height / 2), d.Value);
    }

    private void ConsumeGold(SetDGoldDetails d)
    {
        Gold.NumberChange(d.Value);
    }

    private void GainDHealth(SetDDHealthDetails d)
    {
        Health.Gain(new Vector2(Screen.width / 2, Screen.height / 2), d.Value);
    }

    private void LoseDHealth(SetDDHealthDetails d)
    {
        Health.NumberChange(d.Value);
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
