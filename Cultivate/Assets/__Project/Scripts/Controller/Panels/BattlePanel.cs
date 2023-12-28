
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
    [SerializeField] private BreathingButton CombatButton;
    [SerializeField] private RectTransform CombatButtonTransform;

    [SerializeField] private Image VictoryStamp;
    [SerializeField] private Transform VictoryStampTranform;

    private static readonly float WinBaseScale = 1f;
    private static readonly float LoseBaseScale = 0.6f;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        EnemyView.SetAddress(_address.Append(".Enemy"));

        CombatButton.RemoveAllListeners();
        CombatButton.AddListener(Combat);

        CombatButton.SetBreathing(true);
    }

    public override void Refresh()
    {
        EnemyView.Refresh();

        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        if (d.GetResult() is { } result)
        {
            HomeHealth.text = result.HomeLeftHp.ToString();
            AwayHealth.text = result.AwayLeftHp.ToString();
            SetVictory(result.HomeVictory);
            if (result.HomeVictory)
            {
                HomeHealth.alpha = 1f;
                AwayHealth.alpha = 0.6f;
            }
            else
            {
                HomeHealth.alpha = 0.6f;
                AwayHealth.alpha = 1f;
            }
        }
        else
        {
            HomeHealth.text = "玩家";
            AwayHealth.text = "怪物";
            HomeHealth.alpha = 1f;
            AwayHealth.alpha = 1f;
            SetVictory(false);
        }
    }

    private void Combat(PointerEventData eventData)
    {
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private bool _homeVictory;
    private Tween _handle;

    private void SetVictory(bool victory)
    {
        SetButtonScale(victory ? WinBaseScale : LoseBaseScale);

        if (_homeVictory == victory)
            return;

        _homeVictory = victory;

        VictoryStamp.color = new Color(1, 1, 1, 0);
        VictoryStampTranform.localScale = Vector3.one * 1.5f;
        _handle?.Kill();

        if (_homeVictory)
        {
            // AudioManager.Play("Stamp");

            _handle = DOTween.Sequence().SetAutoKill()
                .Append(VictoryStamp.DOFade(1, 0.15f).SetEase(Ease.InQuad))
                .Append(VictoryStampTranform.DOScale(1, 0.15f).SetEase(Ease.InQuad));
            _handle.Restart();
        }
    }

    private Tween _buttonScaleHandle;
    private void SetButtonScale(float scale)
    {
        _buttonScaleHandle?.Kill();
        _buttonScaleHandle = CombatButtonTransform.DOScale(scale, 0.2f).SetAutoKill();
        _buttonScaleHandle.Restart();
    }

    public override Tween ShowAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayBattleBGM)
            .Append(base.ShowAnimation());
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayJingJieBGM)
            .Append(base.HideAnimation());
    }

    private void PlayBattleBGM()
    {
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        if (d.GetEnemy().IsBoss())
        {
            AudioManager.Play("BGMBoss");
        }
        else if (d.GetEnemy().IsElite())
        {
            AudioManager.Play(RandomManager.value < 0.5f ? "BGMElite1" : "BGMElite2");
        }
    }

    private void PlayJingJieBGM()
    {
        if (RunManager.Instance == null || RunManager.Instance.Environment == null)
            return;
        JingJie jingJie = RunManager.Instance.Environment.Map.GetJingJie();
        AudioEntry audio = Encyclopedia.AudioFromJingJie(jingJie);
        AudioManager.Play(audio);
    }
}
