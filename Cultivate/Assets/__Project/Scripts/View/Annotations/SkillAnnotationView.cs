
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

        object obj = Get<object>();

        ISkillModel skill = null;

        if (obj is ISkillModel s)
        {
            skill = s;
        }
        else if (obj is SkillSlot slot)
        {
            skill = slot.Skill;
        }

        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        TitleText.text = skill.GetName();

        List<SkillType> skillTypes = skill.GetSkillTypeComposite().SkillTypes;
        for (int i = 0; i < TypeTagList.Length; i++)
            TypeTagList[i].SetText(i < skillTypes.Count ? skillTypes[i]._name : null);

        DescriptionText.text = skill.GetAnnotationText();

        string trivia = skill.GetTrivia();
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }
}
