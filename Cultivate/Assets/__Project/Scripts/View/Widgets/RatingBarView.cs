
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class RatingBarView : MonoBehaviour
{
    [SerializeField] private RatingBarSegment[] RatingBarSegments;

    private RatingBarModel _ratingBarModel;
    public Neuron<int> ValueChangedNeuron = new();

    private void Awake()
    {
        foreach (var segment in RatingBarSegments)
        {
            segment.PropagatePointer._onPointerEnter = PointerEnterSegment;
        }
    }
    
    public void SetModel(RatingBarModel ratingBarModel)
    {
        _ratingBarModel = ratingBarModel;

        for (int i = 0; i < RatingBarSegments.Length; i++)
        {
            bool show = i < ratingBarModel.MaxValue;
            RatingBarSegments[i].gameObject.SetActive(show);
            if (!show)
                continue;

            bool hasPoint = i < ratingBarModel.Value;
            RatingBarSegments[i].HasPoint = hasPoint;

            bool isCritical = ratingBarModel.CriticalValues != null && System.Array.IndexOf(ratingBarModel.CriticalValues, i + 1) != -1;
            RatingBarSegments[i].IsCritical = isCritical;
        }
    }

    public void SetValue(int value)
    {
        _ratingBarModel.Value = value;
        for (int i = 0; i < RatingBarSegments.Length; i++)
        {
            bool hasPoint = i < _ratingBarModel.Value;
            RatingBarSegments[i].HasPoint = hasPoint;
        }
        
        ValueChangedNeuron.Invoke(value);
    }

    public void PointerEnterSegment(PointerEventData d)
    {
        int index = FindSegmentIndex(d);
        SetValue(index + 1);
    }

    private int FindSegmentIndex(PointerEventData d)
    {
        GameObject target = d.pointerEnter;
        for (int i = 0; i < RatingBarSegments.Length; i++)
        {
            if (RatingBarSegments[i].gameObject == target)
                return i;
        }

        return -1;
    }
}