
using TMPro;
using UnityEngine;

public class PuzzlePanel : Panel
{
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Indicator;
    // [SerializeField] private Button PassButton;
    // [SerializeField] private Button GiveUpButton;
    [SerializeField] private PuzzleEntityView Home;
    [SerializeField] private PuzzleEntityView Away;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        
        Home.SetAddress(_address.Append(".Home"));
        Away.SetAddress(_address.Append(".Away"));
        
        // CombatButton.RemoveAllListeners();
        // CombatButton.AddListener(Combat);
        //
        // CombatButton.SetBreathing(true);
    }

    public override void Refresh()
    {
        Home.Refresh();
        Away.Refresh();

        PuzzlePanelDescriptor d = _address.Get<PuzzlePanelDescriptor>();
        if (d.GetResult() is { } result)
        {
            Description.text = d.GetDescription();
            Indicator.text = result.HomeLeftHp.ToString();
            
            // SetVictory(result.HomeVictory);
            // if (result.HomeVictory)
            // {
            //     HomeHealth.alpha = 1f;
            //     AwayHealth.alpha = 0.6f;
            // }
            // else
            // {
            //     HomeHealth.alpha = 0.6f;
            //     AwayHealth.alpha = 1f;
            // }
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

    // private void Combat(PointerEventData eventData)
    // {
    //     RunManager.Instance.Environment.ReceiveSignalProcedure(new ClickBattleSignal());
    //     BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
    //     d.Combat();
    //     CanvasManager.Instance.RunCanvas.Refresh();
    // }
    //
    // private bool _homeVictory;
    // private Tween _handle;
    //
    // private void SetVictory(bool victory)
    // {
    //     SetButtonScale(victory ? WinBaseScale : LoseBaseScale);
    //     
    //     if (_homeVictory == victory)
    //         return;
    //     
    //     _homeVictory = victory;
    //     
    //     VictoryStamp.color = new Color(1, 1, 1, 0);
    //     VictoryStampTranform.localScale = Vector3.one * 1.5f;
    //     _handle?.Kill();
    //     
    //     if (_homeVictory)
    //     {
    //         // AudioManager.Play("Stamp");
    //     
    //         _handle = DOTween.Sequence().SetAutoKill()
    //             .Append(VictoryStamp.DOFade(1, 0.15f).SetEase(Ease.InQuad))
    //             .Append(VictoryStampTranform.DOScale(1, 0.15f).SetEase(Ease.InQuad));
    //         _handle.Restart();
    //     }
    // }
    //
    // private Tween _buttonScaleHandle;
    // private void SetButtonScale(float scale)
    // {
    //     _buttonScaleHandle?.Kill();
    //     _buttonScaleHandle = CombatButtonTransform.DOScale(scale, 0.2f).SetAutoKill();
    //     _buttonScaleHandle.Restart();
    // }
    //
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
    //
    // private void PlayBattleBGM()
    // {
    //     int index = RandomManager.Range(0, 3);
    //     string bgm = new string[] { "BGMBoss", "BGMElite1", "BGMElite2" }[index];
    //     AudioManager.Play(bgm);
    // }
    //
    // private void PlayJingJieBGM()
    // {
    //     if (RunManager.Instance == null || RunManager.Instance.Environment == null)
    //         return;
    //     JingJie jingJie = RunManager.Instance.Environment.Map.JingJie;
    //     AudioEntry audio = Encyclopedia.AudioFromJingJie(jingJie);
    //     AudioManager.Play(audio);
    // }
}
