
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
        
        MingYuan.Configure(RunManager.Instance.Environment.GetMingYuan,
            IncrementOne,
            RunManager.Instance.Environment.Home.MingYuan.GetMingYuanPenaltyText);
        // Gold.Configure(RunManager.Instance.Environment.Gold.ToString, () => "金钱");
        // Health.Configure(RunManager.Instance.Environment.Home.GetFinalHealth().ToString, () => "生命值上限");
    }

    private void IncrementOne(BoundedInt i)
        => i.Curr++;

    private void OnEnable()
    {
        CanvasManager.Instance.RunCanvas.MingYuanChangedStagingNeuron.Add(EmitMingYuan);
    }

    private void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.MingYuanChangedStagingNeuron.Remove(EmitMingYuan);
    }

    private void EmitMingYuan(SetDMingYuanDetails d)
    {
        if (d.Value <= 0) return;
        MingYuan.Emit(Vector2.zero, d.Value);
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
