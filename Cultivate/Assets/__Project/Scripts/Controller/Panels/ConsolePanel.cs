
using System;
using CLLibrary;
using Cysharp.Threading.Tasks.Triggers;
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

    public Button Button1;
    public Button Button5;
    public Button Button10;
    public Button Button25;
    public Button Button50;
    public Button Button100;

    public Button ButtonShowGRResult;
    public Button ButtonDoNotShow;

    public Button PrintJsonButton;

    public TMP_Text GRResultText;

    public Button ToggleButton;

    private void Update() => _update?.Invoke();

    public override Tween EnterIdle()
        => DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => GetRect().anchoredPosition = new Vector2(GetRect().anchoredPosition.x, 243f));

    public override Tween EnterHide()
        => DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => GetRect().anchoredPosition = new Vector2(GetRect().anchoredPosition.x, 840f));

    public override void AwakeFunction()
    {
        base.AwakeFunction();

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
        
        
        Button1.onClick.RemoveAllListeners();
        Button1.onClick.AddListener(() => Time.timeScale = 0.01f);
        Button5.onClick.RemoveAllListeners();
        Button5.onClick.AddListener(() => Time.timeScale = 0.05f);
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
        
        PrintJsonButton.onClick.RemoveAllListeners();
        PrintJsonButton.onClick.AddListener(RunManager.Instance.Environment.PrintJson);
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
        RunEnvironment env = RunManager.Instance.Environment;
        MingYuanText.text = env.GetMingYuan().ToString();
        GoldText.text = env.GetGold().Curr.ToString();
        HealthText.text = env.Home.GetHealth().ToString();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.GainMingYuanNeuron.Add(RefreshMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(RefreshMingYuan);
        RunManager.Instance.Environment.GainGoldNeuron.Add(RefreshGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(RefreshGold);
        RunManager.Instance.Environment.GainDHealthNeuron.Add(RefreshDHealth);
        RunManager.Instance.Environment.LoseDHealthNeuron.Add(RefreshDHealth);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainMingYuanNeuron.Remove(RefreshMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(RefreshMingYuan);
        RunManager.Instance.Environment.GainGoldNeuron.Remove(RefreshGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Remove(RefreshGold);
        RunManager.Instance.Environment.GainDHealthNeuron.Remove(RefreshDHealth);
        RunManager.Instance.Environment.LoseDHealthNeuron.Remove(RefreshDHealth);
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
        HealthText.text = env.Home.GetHealth().ToString();
    }

    private void AddMingYuan()
    {
        RunManager.Instance.Environment.SetDMingYuanProcedure(1);
    }

    private void ReduceMingYuan()
    {
        RunManager.Instance.Environment.SetDMingYuanProcedure(-1);
    }

    private void AddGold()
    {
        RunManager.Instance.Environment.SetDGoldProcedure(10);
    }

    private void ReduceGold()
    {
        RunManager.Instance.Environment.SetDGoldProcedure(-10);
    }

    private void AddHealth()
    {
        RunManager.Instance.Environment.SetDHealthProcedure(10);
    }

    private void ReduceHealth()
    {
        RunManager.Instance.Environment.SetDHealthProcedure(-10);
    }

    private void JingJieChanged(int jingJie)
    {
        IEntity entity = RunManager.Instance.Environment.Home;
        entity.SetJingJie(jingJie);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void DrawSkill()
    {
        RunManager.Instance.Environment.DrawSkillProcedure(SkillEntryDescriptor.FromJingJie(RunManager.Instance.Environment.JingJie));
        // RunManager.Instance.Environment.DrawSkillsProcedure(new SkillEntryCollectionDescriptor(jingJie: RunManager.Instance.Environment.JingJie, count: 5));
    }
}
