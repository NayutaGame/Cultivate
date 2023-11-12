using TMPro;
using UnityEngine;

public class FormationAnnotation : MonoBehaviour, IAddress
{
    [SerializeField] protected RectTransform _rectTransform;
    [SerializeField] private TMP_Text TitleText;
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

        object obj = Get<object>();

        FormationEntry formationEntry = obj as FormationEntry ?? (obj as Formation)?.Entry;

        if (formationEntry == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        TitleText.text = $"{formationEntry.GetJingJie()} {formationEntry.GetName()}\n\n{formationEntry.GetConditionDescription()}\n\n{formationEntry.GetRewardDescription()}";
        // DescriptionText.text = formation.GetAnnotationText();

        string trivia = formationEntry.GetTrivia();
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
