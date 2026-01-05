
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tween = DG.Tweening.Tween;

public class ConsolePanel : Panel, Addressable
{
    public GameObject FullContent;
    
    [TabGroup("Left")] public TMP_Text MingYuanText;
    [TabGroup("Left")] [SerializeField] private Button AddMingYuanButton;
    [TabGroup("Left")] [SerializeField] private Button ReduceMingYuanButton;
    [TabGroup("Left")] public TMP_Text GoldText;
    [TabGroup("Left")] [SerializeField] private Button AddGoldButton;
    [TabGroup("Left")] [SerializeField] private Button ReduceGoldButton;
    [TabGroup("Left")] public TMP_Text HealthText;
    [TabGroup("Left")] [SerializeField] private Button AddHealthButton;
    [TabGroup("Left")] [SerializeField] private Button ReduceHealthButton;

    [TabGroup("Left")] public TMP_Dropdown JingJieDropdown;
    [TabGroup("Left")] public Button DrawSkillButton;
    [TabGroup("Left")] public Button RemoveSkillButton;

    private ListModelWithSearchBar<SkillEntry> SkillListModel;
    [TabGroup("Left")] public ListViewWithSearchBar SkillBrowser;

    [TabGroup("Mid")] public Button SetAllLocationsAvailableButton;
    [TabGroup("Mid")] public TMP_InputField LadderInputField;
    [TabGroup("Mid")] public Button ExitRoomButton;

    private ListModelWithSearchBar<RoomEntry> RoomListModel;
    [TabGroup("Mid")] public ListViewWithSearchBar RoomBrowser;

    [TabGroup("Right")] public TMP_InputField TesterNoteInputField;
    [TabGroup("Right")] public Button QuickUpvoteButton;
    [TabGroup("Right")] public Button QuickDownvoteButton;
    [TabGroup("Right")] public Button CopyReportButton;
    
    [TabGroup("Right")] public Button Button1;
    [TabGroup("Right")] public Button Button5;
    [TabGroup("Right")] public Button Button10;
    [TabGroup("Right")] public Button Button25;
    [TabGroup("Right")] public Button Button50;
    [TabGroup("Right")] public Button Button100;

    [TabGroup("Right")] public Button ButtonShowGRResult;
    [TabGroup("Right")] public Button ButtonDoNotShow;
    [TabGroup("Right")] public Button PrintJsonButton;
    [TabGroup("Right")] public Button WriteIntoEditableButton;
    
    [TabGroup("Extra")] public TMP_Text GRResultText;
    [TabGroup("Extra")] public Button ToggleButton;

    private void Update() => _update?.Invoke();

    public override Tween EnterIdle()
        => DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => FullContent.gameObject.SetActive(true));

    public override Tween EnterHide()
        => DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => FullContent.gameObject.SetActive(false));


    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "SkillListModel",               thisObject => ((ConsolePanel)thisObject).SkillListModel },
        { "RoomListModel",                thisObject => ((ConsolePanel)thisObject).RoomListModel },
    };
    public object Get(string s) => Accessor[s](this);
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        SkillListModel = new ListModelWithSearchBar<SkillEntry>(Encyclopedia.SkillCategory.List);
        
        SkillBrowser.SetAddress("Canvas.ConsolePanel.SkillListModel");
        SkillBrowser.CheckAwake();

        RoomListModel = new ListModelWithSearchBar<RoomEntry>(Encyclopedia.RoomCategory.List);

        RoomBrowser.SetAddress("Canvas.ConsolePanel.RoomListModel");
        RoomBrowser.CheckAwake();

        JingJieDropdown.options = new();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.GetName())));

        JingJieDropdown.onValueChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);
        
        
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
        PrintJsonButton.onClick.AddListener(PrintJson);
        
        WriteIntoEditableButton.onClick.RemoveAllListeners();
        WriteIntoEditableButton.onClick.AddListener(WriteIntoEditable);
        
        TesterNoteInputField.onEndEdit.RemoveAllListeners();
        TesterNoteInputField.onEndEdit.AddListener(OnTesterNoteInputFieldEndEdit);
        
        QuickUpvoteButton.onClick.RemoveAllListeners();
        QuickUpvoteButton.onClick.AddListener(QuickUpvote);
        
        QuickDownvoteButton.onClick.RemoveAllListeners();
        QuickDownvoteButton.onClick.AddListener(QuickDownvote);
        
        CopyReportButton.onClick.RemoveAllListeners();
        CopyReportButton.onClick.AddListener(CopyReport);

        ToggleButton.onClick.RemoveAllListeners();
        ToggleButton.onClick.AddListener(() => ToggleShowing());
        
        RunManager.Instance.RegisteredRunEnvironmentNeuron.Join(RegisteredRunEnvironment);
        RunManager.Instance.UnregisteredRunEnvironmentNeuron.Join(UnregisteredRunEnvironment);
    }

    private Action _update;

    private void PrintJson()
    {
        RunManager.Instance.Environment.PrintJson();
    }

    private void WriteIntoEditable()
    {
        RunManager.Instance.Environment.WriteIntoEditable();
    }

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
        if (env != null)
        {
            MingYuanText.text = env.GetMingYuan().ToString();
            GoldText.text = env.GetGold().Curr.ToString();
            HealthText.text = env.Home.GetHealth().ToString();
        }
        else
        {
            MingYuanText.text = "环境未就绪";
            GoldText.text = "环境未就绪";
            HealthText.text = "环境未就绪";
        }
    }

    private void RegisteredRunEnvironment(RunEnvironment env)
    {
        env.GainMingYuanNeuron.Add(RefreshMingYuan);
        env.LoseMingYuanNeuron.Add(RefreshMingYuan);
        env.GainGoldNeuron.Add(RefreshGold);
        env.LoseGoldNeuron.Add(RefreshGold);
        env.GainHealthNeuron.Add(RefreshDHealth);
        env.LoseHealthNeuron.Add(RefreshDHealth);
        env.AppendReportNeuron.Add(OnAppendReport);
        
        AddMingYuanButton.onClick.AddListener(AddMingYuan);
        ReduceMingYuanButton.onClick.AddListener(ReduceMingYuan);
        AddGoldButton.onClick.AddListener(AddGold);
        ReduceGoldButton.onClick.AddListener(ReduceGold);
        AddHealthButton.onClick.AddListener(AddHealth);
        ReduceHealthButton.onClick.AddListener(ReduceHealth);

        DrawSkillButton.onClick.AddListener(DrawSkill);
        SkillBrowser.Browser.NeuronBundle.LeftClickNeuron.Add(PickSkill);
        RemoveSkillButton.onClick.AddListener(RemoveSkill);
        
        SetAllLocationsAvailableButton.onClick.AddListener(SetAllLocationsAvailable);
        RoomBrowser.Browser.NeuronBundle.LeftClickNeuron.Add(EnterRoom);
        ExitRoomButton.onClick.AddListener(ExitRoom);
    }

    private void UnregisteredRunEnvironment(RunEnvironment env)
    {
        env.GainMingYuanNeuron.Remove(RefreshMingYuan);
        env.LoseMingYuanNeuron.Remove(RefreshMingYuan);
        env.GainGoldNeuron.Remove(RefreshGold);
        env.LoseGoldNeuron.Remove(RefreshGold);
        env.GainHealthNeuron.Remove(RefreshDHealth);
        env.LoseHealthNeuron.Remove(RefreshDHealth);
        env.AppendReportNeuron.Remove(OnAppendReport);
        
        AddMingYuanButton.onClick.RemoveAllListeners();
        ReduceMingYuanButton.onClick.RemoveAllListeners();
        AddGoldButton.onClick.RemoveAllListeners();
        ReduceGoldButton.onClick.RemoveAllListeners();
        AddHealthButton.onClick.RemoveAllListeners();
        ReduceHealthButton.onClick.RemoveAllListeners();

        DrawSkillButton.onClick.RemoveAllListeners();
        SkillBrowser.Browser.NeuronBundle.LeftClickNeuron.Remove(PickSkill);
        RemoveSkillButton.onClick.RemoveAllListeners();
        
        SetAllLocationsAvailableButton.onClick.RemoveAllListeners();
        RoomBrowser.Browser.NeuronBundle.LeftClickNeuron.Remove(EnterRoom);
        ExitRoomButton.onClick.RemoveAllListeners();
    }

    private void OnEnable()
    {
        RefreshInfo();
    }

    private void OnDisable()
    {
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
        JingJie currJingJie = RunManager.Instance.Environment.Map.JingJie;
        SkillEntryQuery query = SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, currJingJie));
        RunManager.Instance.Environment.DrawSkillProcedure(query, currJingJie);
    }

    private void PickSkill(InteractBehaviour ib, PointerEventData d)
    {
        SkillEntry skillEntry = ib.Get<SkillEntry>();
        JingJie jingJie = skillEntry.LowestJingJie;
        RunManager.Instance.Environment.PickSkillProcedure(skillEntry, jingJie);
    }

    private void RemoveSkill()
    {
        int handCount = RunManager.Instance.Environment.Hand.Count();
        if (handCount == 0)
            return;
        DeckIndex lastSkillInHand = DeckIndex.FromHand(handCount - 1);
        RunManager.Instance.Environment.RemoveSkillProcedure(lastSkillInHand);
    }

    private void SetAllLocationsAvailable()
    {
        RunManager.Instance.Environment.Map.SetAllLocationsAvailable();
        CanvasManager.Instance.RunCanvas.MapPanel.LocationList.Refresh();
    }

    private void EnterRoom(InteractBehaviour ib, PointerEventData d)
    {
        RoomEntry roomEntry = ib.Get<RoomEntry>();
        int ladder = int.TryParse(LadderInputField.text, out int result) ? result : 0;
        RunManager.Instance.Environment.Map.EnterRoomProcedure(null, roomEntry, ladder);
    }

    private void ExitRoom()
    {
        RunManager.Instance.Environment.Map.ExitRoomProcedure();
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
