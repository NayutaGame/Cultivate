
using TMPro;
using UnityEngine;

public class SkillPreview : MonoBehaviour, IAddress
{
    [SerializeField] protected RectTransform _rectTransform;
    [SerializeField] private TMP_Text AnnotationText;

    public SkillView SkillView;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void SetAddress(Address address)
    {
        _address = address;
        SkillView.SetAddress(address);
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

        AnnotationText.text = skill.GetAnnotationText();
        SkillView.Refresh();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
