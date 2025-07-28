
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
    public Button CheatButton;
    public Button WinButton;
    public Button LoseButton;

    public Button Button1;
    public Button Button5;
    public Button Button10;
    public Button Button25;
    public Button Button50;
    public Button Button100;

    public Button ButtonShowGRResult;
    public Button ButtonDoNotShow;

    public Button PrintJsonButton;
    public Button WriteIntoEditable;

    public TMP_Text GRResultText;

    public Button ToggleButton;

    public TMP_InputField TesterNoteInputField;
    public Button QuickUpvoteButton;
    public Button QuickDownvoteButton;
    public Button CopyReportButton;

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
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.GetName())));

        JingJieDropdown.onValueChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);

        DrawSkillButton.onClick.RemoveAllListeners();
        DrawSkillButton.onClick.AddListener(DrawSkill);

        CheatButton.onClick.RemoveAllListeners();
        CheatButton.onClick.AddListener(Cheat);

        WinButton.onClick.RemoveAllListeners();
        WinButton.onClick.AddListener(Win);

        LoseButton.onClick.RemoveAllListeners();
        LoseButton.onClick.AddListener(Lose);

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
        
        WriteIntoEditable.onClick.RemoveAllListeners();
        WriteIntoEditable.onClick.AddListener(RunManager.Instance.Environment.WriteIntoEditable);
        
        TesterNoteInputField.onEndEdit.RemoveAllListeners();
        TesterNoteInputField.onEndEdit.AddListener(OnTesterNoteInputFieldEndEdit);
        
        QuickUpvoteButton.onClick.RemoveAllListeners();
        QuickUpvoteButton.onClick.AddListener(QuickUpvote);
        
        QuickDownvoteButton.onClick.RemoveAllListeners();
        QuickDownvoteButton.onClick.AddListener(QuickDownvote);
        
        CopyReportButton.onClick.RemoveAllListeners();
        CopyReportButton.onClick.AddListener(CopyReport);
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

    private void RefreshInfo()
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
        RunManager.Instance.Environment.GainHealthNeuron.Add(RefreshDHealth);
        RunManager.Instance.Environment.LoseHealthNeuron.Add(RefreshDHealth);
        RunManager.Instance.Environment.AppendReportNeuron.Add(OnAppendReport);
        RefreshInfo();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainMingYuanNeuron.Remove(RefreshMingYuan);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(RefreshMingYuan);
        RunManager.Instance.Environment.GainGoldNeuron.Remove(RefreshGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Remove(RefreshGold);
        RunManager.Instance.Environment.GainHealthNeuron.Remove(RefreshDHealth);
        RunManager.Instance.Environment.LoseHealthNeuron.Remove(RefreshDHealth);
        RunManager.Instance.Environment.AppendReportNeuron.Remove(OnAppendReport);
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
        RunManager.Instance.Environment.GainHealthProcedure(10);
    }

    private void ReduceHealth()
    {
        RunManager.Instance.Environment.LoseHealthProcedure(10);
    }

    private void JingJieChanged(int jingJie)
    {
        IEntity entity = RunManager.Instance.Environment.Home;
        entity.SetJingJie(jingJie);
    }

    private void DrawSkill()
    {
        SkillEntryDescriptor descriptor = SkillEntryDescriptor.FromJingJie(RunManager.Instance.Environment.JingJie);
        
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        b.Create(descriptor.JingJie);
        b.Add();
        b.Invoke();
        // RunManager.Instance.Environment.DrawSkillsProcedure(new SkillEntryCollectionDescriptor(jingJie: RunManager.Instance.Environment.JingJie, count: 5));
    }

    private void Cheat()
    {
        GainSkillBuilder b = new();
        b.Pick(SkillEntry.FromName("作弊"));
        b.Create();
        b.Add();
        b.Invoke();
        // RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("玄武吐息法"));
        // RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("吞天"));
        // RunManager.Instance.Environment.AddSkillProcedure(SkillEntry.FromName("童趣"));
    }

    private void Win()
    {
        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Victorious);
    }

    private void Lose()
    {
        RunManager.Instance.Environment.CommitRunProcedure(RunResult.RunOutcome.Defeated);
    }

    private void OnTesterNoteInputFieldEndEdit(string value)
    {
        if (!AppManager.Instance.AudienceIsTester()) return;
        RunManager.Instance.Environment.GetRunReport().GetCurrReport().TesterNote = value;
    }

    private void QuickUpvote()
    {
        if (!AppManager.Instance.AudienceIsTester()) return;
        string newNote = RunManager.Instance.Environment.GetRunReport().GetCurrReport().TesterNote + ", 赞";
        TesterNoteInputField.text = newNote;
        RunManager.Instance.Environment.GetRunReport().GetCurrReport().TesterNote = newNote;
    }

    private void QuickDownvote()
    {
        if (!AppManager.Instance.AudienceIsTester()) return;
        string newNote = RunManager.Instance.Environment.GetRunReport().GetCurrReport().TesterNote + ", 踩";
        TesterNoteInputField.text = newNote;
        RunManager.Instance.Environment.GetRunReport().GetCurrReport().TesterNote = newNote;
    }

    private void CopyReport()
    {
        if (!AppManager.Instance.AudienceIsTester()) return;
        RunManager.Instance.Environment.GetRunReport().CopyRunReportToClipboard();
    }

    private void OnAppendReport()
    {
        TesterNoteInputField.text = null;
    }
}
