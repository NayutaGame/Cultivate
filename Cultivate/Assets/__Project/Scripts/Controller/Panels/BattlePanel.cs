
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    [SerializeField] private BattleEntityView EnemyView;
    [SerializeField] private TMP_Text HomeHealth;
    [SerializeField] private TMP_Text AwayHealth;

    [SerializeField] public CombatButton CombatButton;
    [SerializeField] private GameObject VictoryStamp;

    private static readonly float WinBaseScale = 1f;
    private static readonly float LoseBaseScale = 0.6f;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        EnemyView.SetAddress(_address.Append(".Enemy"));

        CombatButton.ClickNeuron.Join(Combat);
    }

    public override void Refresh()
    {
        EnemyView.Refresh();

        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        if (d.GetResult() is { } result)
        {
            HomeHealth.text = result.HomeLeftHp.ToString();
            AwayHealth.text = result.AwayLeftHp.ToString();
            SetVictory(result.Flag == 1);
        }
        else
        {
            HomeHealth.text = "玩家";
            AwayHealth.text = "怪物";
            SetVictory(false);
            
            HomeHealth.alpha = 1f;
            AwayHealth.alpha = 1f;
        }
        
        CanvasManager.Instance.RefreshGuide();
    }

    private void Combat(PointerEventData eventData)
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(new ClickBattleSignal());
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void SetVictory(bool victory)
    {
        CombatButton.SetBaseScale(victory ? WinBaseScale : LoseBaseScale);
        VictoryStamp.SetActive(victory);
        HomeHealth.alpha = victory ? 1f : 0.6f;
        AwayHealth.alpha = victory ? 0.6f : 1f;
    }

    public override Tween ShowAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayBattleBGM)
            .Append(EnemyView.ShowAnimation())
            .Join(base.ShowAnimation());
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayJingJieBGM)
            .Append(EnemyView.HideAnimation())
            .Join(base.HideAnimation());
    }

    private void PlayBattleBGM()
    {
        int index = RandomManager.Range(0, 3);
        string bgm = new string[] { "BGMBoss", "BGMElite1", "BGMElite2" }[index];
        AudioManager.Play(bgm);
    }

    private void PlayJingJieBGM()
    {
        if (RunManager.Instance == null || RunManager.Instance.Environment == null)
            return;
        JingJie jingJie = RunManager.Instance.Environment.Map.JingJie;
        AudioEntry audio = Encyclopedia.AudioFromJingJie(jingJie);
        AudioManager.Play(audio);
    }
}
