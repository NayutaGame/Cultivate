using System.Collections;
using System.Collections.Generic;
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

    public override Tween GetShowTween()
        => DOTween.Sequence().SetAutoKill()
            .Append(_rectTransform.DOAnchorPosY(243f, 0.3f).SetEase(Ease.OutQuad));

    public override Tween GetHideTween()
        => DOTween.Sequence().SetAutoKill()
            .Append(_rectTransform.DOAnchorPosY(771f, 0.3f).SetEase(Ease.InQuad));

    public override void Configure()
    {
        base.Configure();

        MingYuanButton.onClick.AddListener(AddMingYuan);
        GoldButton.onClick.AddListener(AddXiuWei);

        HealthInputField.onValueChanged.AddListener(HealthChanged);

        JingJieDropdown.options = new();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);

        DrawSkillButton.onClick.AddListener(DrawSkill);
    }

    public override void Refresh()
    {
        base.Refresh();

        MingYuanText.text = RunManager.Instance.MingYuan.ToString();
        GoldText.text = RunManager.Instance.XiuWei.ToString();

        IEntityModel entity = RunManager.Instance.Battle.Hero;
        HealthInputField.SetTextWithoutNotify(entity.GetBaseHealth().ToString());
        JingJieDropdown.SetValueWithoutNotify(entity.GetJingJie());
    }

    public void AddMingYuan()
    {
        RunManager.Instance.AddMingYuan();
        Refresh();
    }

    private void AddXiuWei()
    {
        RunManager.Instance.AddXiuWei();
        Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        IEntityModel entity = RunManager.Instance.Battle.Hero;
        entity.SetBaseHealth(health);
        RunCanvas.Instance.Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        IEntityModel entity = RunManager.Instance.Battle.Hero;
        entity.SetJingJie(jingJie);
        RunCanvas.Instance.Refresh();
    }

    private void DrawSkill()
    {
        RunManager.Instance.Battle.SkillInventory.TryDrawSkill(out RunSkill skill, jingJie: RunManager.Instance.Map.JingJie);
        RunCanvas.Instance.Refresh();
    }
}
