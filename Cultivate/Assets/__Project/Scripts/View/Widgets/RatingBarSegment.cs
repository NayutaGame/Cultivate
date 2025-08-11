using System;
using UnityEngine;
using UnityEngine.UI;

public class RatingBarSegment : MonoBehaviour
{
    [SerializeField] private Image RectImage;
    [SerializeField] public PropagatePointer PropagatePointer;
    
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
        // 0 nonCritical noPoint
        // 1 isCritical noPoint
        // 2 nonCritical hasPoint
        // 3 isCritical hasPoint
        int index = 2 * (_hasPoint ? 1 : 0) + (_isCritical ? 1 : 0);
        RectImage.sprite = CanvasManager.Instance.RatingBarSegmentSprites[index];
        // RectImage.color = 
    }
}