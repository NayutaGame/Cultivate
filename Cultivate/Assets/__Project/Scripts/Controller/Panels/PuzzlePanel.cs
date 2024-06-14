
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePanel : Panel
{
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Condition;
    [SerializeField] private TMP_Text Indicator;
    [SerializeField] private GameObject PassStamp;
    [SerializeField] private Button Button;
    [SerializeField] private TMP_Text ButtonText;
    [SerializeField] private PuzzleEntityView Home;
    [SerializeField] private PuzzleEntityView Away;

    private static readonly float WinBaseScale = 1f;
    private static readonly float LoseBaseScale = 0.6f;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        
        Home.SetAddress(_address.Append(".Home"));
        Away.SetAddress(_address.Append(".Away"));
        
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Callback);
    }

    public override void Refresh()
    {
        Home.Refresh();
        Away.Refresh();

        PuzzlePanelDescriptor d = _address.Get<PuzzlePanelDescriptor>();
        if (d.GetResult() is { } result)
        {
            Description.text = d.GetDescription();
            Condition.text = d.GetCondition();
            Indicator.text = result.HomeLeftHp.ToString();
            SetVictory(result.Flag == 1);
        }
        else
        {
            // HomeHealth.text = "玩家";
            // AwayHealth.text = "怪物";
            // HomeHealth.alpha = 1f;
            // AwayHealth.alpha = 1f;
            // SetVictory(false);
        }
        
        CanvasManager.Instance.RefreshGuide();
    }

    private void SetVictory(bool victory)
    {
        PassStamp.SetActive(victory);
        Indicator.alpha = victory ? 1f : 0.6f;
        ButtonText.text = victory ? "通过" : "尝试其他方法";
    }

    private void Callback()
    {
        PuzzlePanelDescriptor d = _address.Get<PuzzlePanelDescriptor>();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new PuzzleResultSignal(d.GetResult().Flag));
        PanelS panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
    }
    
    // public override Tween ShowAnimation()
    // {
    //     return DOTween.Sequence()
    //         .AppendCallback(PlayBattleBGM)
    //         .Append(EnemyView.ShowAnimation())
    //         .Join(base.ShowAnimation());
    // }
    //
    // public override Tween HideAnimation()
    // {
    //     return DOTween.Sequence()
    //         .AppendCallback(PlayJingJieBGM)
    //         .Append(EnemyView.HideAnimation())
    //         .Join(base.HideAnimation());
    // }
}
