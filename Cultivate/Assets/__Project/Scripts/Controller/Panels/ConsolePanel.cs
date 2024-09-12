
using System;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tween = DG.Tweening.Tween;

public class ConsolePanel : Panel
{
    public TMP_Text MingYuanText;
    [SerializeField] private Button AddMingYuanButton;
    [SerializeField] private Button ReduceMingYuanButton;
    public TMP_Text GoldText;
    [SerializeField] private Button AddGoldButton;
    [SerializeField] private Button ReduceGoldButton;
    public TMP_Text HealthText;
    [SerializeField] private Button AddHealthButton;
    [SerializeField] private Button ReduceHealthButton;
    
    public TMP_Dropdown JingJieDropdown;
    public Button DrawSkillButton;

    public Button Button10;
    public Button Button25;
    public Button Button50;
    public Button Button100;

    public Button ButtonShowGRResult;
    public Button ButtonDoNotShow;

    public TMP_Text GRResultText;

    public Button ToggleButton;

    private void Update() => _update?.Invoke();

    public override Tween ShowTween()
        => DOTween.Sequence().SetAutoKill()
            .Append(RectTransform.DOAnchorPosY(243f, 0.3f).SetEase(Ease.OutQuad));

    public override Tween HideTween()
        => DOTween.Sequence().SetAutoKill()
            .Append(RectTransform.DOAnchorPosY(771f, 0.3f).SetEase(Ease.InQuad));

    public override void Configure()
    {
        base.Configure();

        AddMingYuanButton.onClick.RemoveAllListeners();
        AddMingYuanButton.onClick.AddListener(AddMingYuan);
        ReduceMingYuanButton.onClick.RemoveAllListeners();
        ReduceMingYuanButton.onClick.AddListener(ReduceMingYuan);
        
        AddGoldButton.onClick.RemoveAllListeners();
        AddGoldButton.onClick.AddListener(AddGold);
        ReduceGoldButton.onClick.RemoveAllListeners();
        ReduceGoldButton.onClick.AddListener(ReduceGold);
        
        AddHealthButton.onClick.RemoveAllListeners();
        AddHealthButton.onClick.AddListener(AddHealth);
        ReduceHealthButton.onClick.RemoveAllListeners();
        ReduceHealthButton.onClick.AddListener(ReduceHealth);

        JingJieDropdown.options = new();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));

        JingJieDropdown.onValueChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);

        DrawSkillButton.onClick.RemoveAllListeners();
        DrawSkillButton.onClick.AddListener(DrawSkill);

        ToggleButton.onClick.RemoveAllListeners();
        ToggleButton.onClick.AddListener(() => ToggleShowing());
        
        Button10.onClick.RemoveAllListeners();
        Button10.onClick.AddListener(() => Time.timeScale = 0.1f);
        Button25.onClick.RemoveAllListeners();
        Button25.onClick.AddListener(() => Time.timeScale = 0.25f);
        Button50.onClick.RemoveAllListeners();
        Button50.onClick.AddListener(() => Time.timeScale = 0.5f);
        Button100.onClick.RemoveAllListeners();
        Button100.onClick.AddListener(() => Time.timeScale = 1);
        
        ButtonShowGRResult.onClick.RemoveAllListeners();
        ButtonShowGRResult.onClick.AddListener(TurnOnShowGRResult);
        ButtonDoNotShow.onClick.RemoveAllListeners();
        ButtonDoNotShow.onClick.AddListener(TurnOffShow);
    }

    private Action _update;

    private void TurnOnShowGRResult()
    {
        GRResultText.gameObject.SetActive(true);
        _update += ShowGRResult;
    }

    private void TurnOffShow()
    {
        GRResultText.gameObject.SetActive(false);
        _update = null;
    }

    private void ShowGRResult()
    {
        GRResultText.text = CanvasManager.Instance.GetGraphicRaycastResult();
    }

    public override void Refresh()
    {
        base.Refresh();

        RunEnvironment env = RunManager.Instance.Environment;
        MingYuanText.text = env.GetMingYuan().ToString();
        GoldText.text = env.GetGold().Curr.ToString();
        HealthText.text = env.Home.GetFinalHealth().ToString();
    }

    private void OnEnable()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanStagingNeuron.Add(RefreshMingYuan);
        CanvasManager.Instance.RunCanvas.LoseMingYuanStagingNeuron.Add(RefreshMingYuan);
        CanvasManager.Instance.RunCanvas.GainGoldStagingNeuron.Add(RefreshGold);
        CanvasManager.Instance.RunCanvas.LoseGoldStagingNeuron.Add(RefreshGold);
        CanvasManager.Instance.RunCanvas.GainDHealthStagingNeuron.Add(RefreshDHealth);
        CanvasManager.Instance.RunCanvas.LoseDHealthStagingNeuron.Add(RefreshDHealth);
    }

    private void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanStagingNeuron.Remove(RefreshMingYuan);
        CanvasManager.Instance.RunCanvas.LoseMingYuanStagingNeuron.Remove(RefreshMingYuan);
        CanvasManager.Instance.RunCanvas.GainGoldStagingNeuron.Remove(RefreshGold);
        CanvasManager.Instance.RunCanvas.LoseGoldStagingNeuron.Remove(RefreshGold);
        CanvasManager.Instance.RunCanvas.GainDHealthStagingNeuron.Remove(RefreshDHealth);
        CanvasManager.Instance.RunCanvas.LoseDHealthStagingNeuron.Remove(RefreshDHealth);
    }

    private void RefreshMingYuan(int value)
    {
        RunEnvironment env = RunManager.Instance.Environment;
        MingYuanText.text = env.GetMingYuan().ToString();
    }

    private void RefreshGold(int value)
    {
        RunEnvironment env = RunManager.Instance.Environment;
        GoldText.text = env.GetGold().Curr.ToString();
    }

    private void RefreshDHealth(int value)
    {
        RunEnvironment env = RunManager.Instance.Environment;
        HealthText.text = env.Home.GetFinalHealth().ToString();
    }

    private void AddMingYuan()
    {
        CanvasManager.Instance.RunCanvas.GainMingYuanProcedure(1);
    }

    private void ReduceMingYuan()
    {
        CanvasManager.Instance.RunCanvas.LoseMingYuanProcedure(-1);
    }

    private void AddGold()
    {
        CanvasManager.Instance.RunCanvas.GainGoldProcedure(10);
    }

    private void ReduceGold()
    {
        CanvasManager.Instance.RunCanvas.LoseGoldProcedure(-10);
    }

    private void AddHealth()
    {
        CanvasManager.Instance.RunCanvas.GainDHealthProcedure(10);
    }

    private void ReduceHealth()
    {
        CanvasManager.Instance.RunCanvas.LoseDHealthProcedure(-10);
    }

    private void JingJieChanged(int jingJie)
    {
        EntityModel entity = RunManager.Instance.Environment.Home;
        entity.SetJingJie(jingJie);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void DrawSkill()
    {
        CanvasManager.Instance.RunCanvas.GainSkillProcedure(CanvasManager.Instance.ScreenCenterInWorld(), SkillEntryDescriptor.FromJingJie(RunManager.Instance.Environment.JingJie));
    }
}
