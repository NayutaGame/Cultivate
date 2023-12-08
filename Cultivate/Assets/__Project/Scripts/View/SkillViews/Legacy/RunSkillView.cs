
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunSkillView : SkillView
{
    [SerializeField] private RectTransform ContentTransform;

    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;
    [SerializeField] private RectTransform MousePivot;

    private Tween _animationHandle;

    private void OnEnable()
    {
        ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
        // if (CanvasGroup != null)
        //     CanvasGroup.blocksRaycasts = true;
    }
}
