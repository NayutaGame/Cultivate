
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillAnnotationView : SimpleView
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TypeTag[] TypeTagList;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = GetSkill();

        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        TitleText.text = skill.GetName();

        List<SkillType> skillTypes = skill.GetSkillTypeComposite().SkillTypes;
        for (int i = 0; i < TypeTagList.Length; i++)
            TypeTagList[i].SetText(i < skillTypes.Count ? skillTypes[i]._name : null);

        DescriptionText.text = skill.GetExplanation();

        SetTrivia(skill.GetTrivia());
    }

    private ISkillModel GetSkill()
    {
        object obj = Get<object>();

        if (obj is ISkillModel s)
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
}
