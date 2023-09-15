using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationView : ItemView,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected RectTransform _rectTransform;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text ConditionText;
    [SerializeField] private TMP_Text RewardText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _rectTransform = GetComponent<RectTransform>();
    }

    public override void Refresh()
    {
        base.Refresh();
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
        RunCanvas.Instance.SetIndexPathForSubFormationPreview(GetAddress());
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
