
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
        RunManager.Instance.Environment.GainMingYuanNeuron.Add(GainMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(MingYuan.LoseNoAnimation);
        RunManager.Instance.Environment.GainGoldNeuron.Add(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(Gold.Lose);
        RunManager.Instance.Environment.GainDHealthNeuron.Add(GainDHealth);
        RunManager.Instance.Environment.LoseDHealthNeuron.Add(Health.Lose);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainMingYuanNeuron.Remove(GainMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(MingYuan.LoseNoAnimation);
        RunManager.Instance.Environment.GainGoldNeuron.Remove(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Remove(Gold.Lose);
        RunManager.Instance.Environment.GainDHealthNeuron.Remove(GainDHealth);
        RunManager.Instance.Environment.LoseDHealthNeuron.Remove(Health.Lose);
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
