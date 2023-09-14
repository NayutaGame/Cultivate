using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationView : MonoBehaviour, IAddress,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected RectTransform _rectTransform;

    private Address _address;
    public Address GetIndexPath() => _address;
    public T Get<T>() => _address.Get<T>();

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text ConditionText;
    [SerializeField] private TMP_Text RewardText;

    public virtual void Configure(Address address)
    {
        _address = address;
        _rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Refresh()
    {
        FormationEntry e = Get<FormationEntry>();
        if (e == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (NameText != null)
            NameText.text = e.GetName();

        if (JingJieText != null)
            JingJieText.text = e.GetJingJie().ToString();

        if (ConditionText != null)
            ConditionText.text = e.GetConditionDescription();

        if (RewardText != null)
            RewardText.text = e.GetRewardDescription();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSubFormationPreview(GetIndexPath());
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForSubFormationPreview(null);
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.UpdateMousePosForSubFormationPreview(eventData.position);
    }
}
