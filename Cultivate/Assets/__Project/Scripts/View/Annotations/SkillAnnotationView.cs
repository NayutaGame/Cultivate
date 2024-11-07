
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillAnnotationView : SimpleView
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private GameObject WuXingGameObject;
    [SerializeField] private TMP_Text WuXingText;
    [SerializeField] private GameObject TypeTagGameObject;
    [SerializeField] private TypeTag[] TypeTagList;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        ISkill skill = GetSkill();

        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        TitleText.text = skill.GetName();
        SetWuXing(skill.GetWuXing());

        List<SkillType> skillTypes = skill.GetSkillTypeComposite().SkillTypes;
        if (skillTypes.Count <= 0)
        {
            TypeTagGameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < TypeTagList.Length; i++)
                TypeTagList[i].SetText(i < skillTypes.Count ? skillTypes[i]._name : null);
            TypeTagGameObject.SetActive(true);
        }

        DescriptionText.text = skill.GetExplanation();

        SetTrivia(skill.GetTrivia());
    }

    private ISkill GetSkill()
    {
        object obj = Get<object>();

        if (obj is ISkill s)
            return s;

        if (obj is SkillSlot slot)
            return slot.Skill;

        return null;
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }

    private void SetWuXing(WuXing? wuXing)
    {
        if (wuXing.HasValue)
        {
            WuXingGameObject.SetActive(true);
            WuXingText.text = wuXing.Value.ToString();
            return;
        }
        
        WuXingGameObject.SetActive(false);
    }
}
