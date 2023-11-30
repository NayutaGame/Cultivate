
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillAnnotation : AnnotationBehaviour
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TypeTag[] TypeTagList;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        Refresh();
    }

    public override void Refresh()
    {
        base.Refresh();

        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        ISkillModel skill = Get<ISkillModel>();

        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

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
