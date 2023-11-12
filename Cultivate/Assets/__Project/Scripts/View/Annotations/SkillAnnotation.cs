
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;

public class SkillAnnotation : MonoBehaviour, IAddress
{
    [SerializeField] protected RectTransform _rectTransform;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TypeTag[] TypeTagList;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
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

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
