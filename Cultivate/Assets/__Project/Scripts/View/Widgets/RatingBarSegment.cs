using System;
using UnityEngine;
using UnityEngine.UI;

public class RatingBarSegment : MonoBehaviour
{
    [SerializeField] private Image RectImage;
    [SerializeField] public PropagatePointer PropagatePointer;
    
    [Header("Colors")]
    [SerializeField] private Color LightColor = Color.white;
    [SerializeField] private Color DarkColor = Color.black;

    [Header("Size")]
    [SerializeField] private float CriticalSize = 20f;
    [SerializeField] private float NonCriticalSize = 15f;
    
    private bool _hasPoint;
    private bool _isCritical;

    public bool HasPoint
    {
        get => _hasPoint;
        set
        {
            _hasPoint = value;
            UpdateVisual();
        }
    }
    
    public bool IsCritical
    {
        get => _isCritical;
        set
        {
            _isCritical = value;
            UpdateVisual();
        }
    }
    
    private void UpdateVisual()
    {
        RectImage.color = _hasPoint ? LightColor : DarkColor;

        RectTransform rectTransform = RectImage.rectTransform;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = _isCritical ? CriticalSize : NonCriticalSize;
        rectTransform.sizeDelta = sizeDelta;
    }
}