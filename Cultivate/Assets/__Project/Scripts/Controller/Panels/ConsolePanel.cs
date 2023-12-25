
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsolePanel : Panel
{
    public TMP_Text MingYuanText;
    public Button MingYuanButton;
    public TMP_Text GoldText;
    public Button GoldButton;
    public TMP_InputField HealthInputField;
    public TMP_Dropdown JingJieDropdown;
    public Button DrawSkillButton;
    public Button AddMechButton;

    public override Tween ShowAnimation()
        => DOTween.Sequence().SetAutoKill()
            .Append(RectTransform.DOAnchorPosY(243f, 0.3f).SetEase(Ease.OutQuad));

    public override Tween HideAnimation()
        => DOTween.Sequence().SetAutoKill()
            .Append(RectTransform.DOAnchorPosY(771f, 0.3f).SetEase(Ease.InQuad));

    public override void Configure()
    {
        base.Configure();

        MingYuanButton.onClick.RemoveAllListeners();
        MingYuanButton.onClick.AddListener(AddMingYuan);

        GoldButton.onClick.RemoveAllListeners();
        GoldButton.onClick.AddListener(AddXiuWei);

        HealthInputField.onValueChanged.RemoveAllListeners();
        HealthInputField.onValueChanged.AddListener(HealthChanged);

        JingJieDropdown.options = new();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));

        JingJieDropdown.onValueChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);

        DrawSkillButton.onClick.RemoveAllListeners();
        DrawSkillButton.onClick.AddListener(DrawSkill);

        AddMechButton.onClick.RemoveAllListeners();
        AddMechButton.onClick.AddListener(AddMech);
    }

    public override void Refresh()
    {
        base.Refresh();

        RunEnvironment env = RunManager.Instance.Environment;
        MingYuanText.text = env.GetMingYuan().ToString();
        GoldText.text = env.Gold.ToString();

        IEntityModel entity = RunManager.Instance.Environment.Home;
        HealthInputField.SetTextWithoutNotify(entity.GetBaseHealth().ToString());
        JingJieDropdown.SetValueWithoutNotify(entity.GetJingJie());
    }

    public void AddMingYuan()
    {
        RunManager.Instance.Environment.GetMingYuan().SetDiff(1);
        Refresh();
    }

    private void AddXiuWei()
    {
        RunManager.Instance.Environment.SetDGold();
        Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        IEntityModel entity = RunManager.Instance.Environment.Home;
        entity.SetBaseHealth(health);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        IEntityModel entity = RunManager.Instance.Environment.Home;
        entity.SetJingJie(jingJie);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void DrawSkill()
    {
        RunManager.Instance.Environment.ForceDrawSkill(jingJie: RunManager.Instance.Environment.Map.GetJingJie());
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void AddMech()
    {
        foreach(MechType m in MechType.Traversal)
            RunManager.Instance.Environment.ForceAddMech(new(m));
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
