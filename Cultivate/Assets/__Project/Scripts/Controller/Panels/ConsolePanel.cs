
using System;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Tween = DG.Tweening.Tween;

public class ConsolePanel : Panel
{
    public TMP_Text MingYuanText;
    [SerializeField] private Button AddMingYuanButton;
    [SerializeField] private Button ReduceMingYuanButton;
    public TMP_Text GoldText;
    public Button GoldButton;
    public TMP_InputField HealthInputField;
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

        GoldButton.onClick.RemoveAllListeners();
        GoldButton.onClick.AddListener(AddGold);

        HealthInputField.onValueChanged.RemoveAllListeners();
        HealthInputField.onValueChanged.AddListener(HealthChanged);

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
        GoldText.text = env.Gold.ToString();

        EntityModel entity = RunManager.Instance.Environment.Home;
        HealthInputField.SetTextWithoutNotify(entity.GetBaseHealth().ToString());
        JingJieDropdown.SetValueWithoutNotify(entity.GetJingJie());
    }

    public void AddMingYuan()
    {
        RunManager.Instance.Environment.SetDMingYuanProcedure(1);
        Refresh();
    }

    public void ReduceMingYuan()
    {
        RunManager.Instance.Environment.SetDMingYuanProcedure(-1);
        Refresh();
    }

    private void AddGold()
    {
        RunManager.Instance.Environment.SetDGoldProcedure(10);
        Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        EntityModel entity = RunManager.Instance.Environment.Home;
        entity.SetBaseHealth(health);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        EntityModel entity = RunManager.Instance.Environment.Home;
        entity.SetJingJie(jingJie);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void DrawSkill()
    {
        RunManager.Instance.Environment.DrawSkillProcedure(SkillDescriptor.FromJingJie(RunManager.Instance.Environment.Map.JingJie));
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
