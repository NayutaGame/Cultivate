
using UnityEngine;
using UnityEngine.EventSystems;

public class CurvedAnimatedListView : AnimatedListView
{
    [SerializeField] private RectTransform _startPoint;
    [SerializeField] private RectTransform _middlePoint;
    [SerializeField] private RectTransform _endPoint;
    [SerializeField] private PropagateDrag _propagateDrag;
    [SerializeField] private float _itemSpacing = 100f;

    private ArcDefinition _arcDefinition;
    private float _arcLength;
    private float _sLength;
    
    private float _s;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _arcDefinition = ArcDefinition.FromThreePoints(_startPoint.anchoredPosition, _middlePoint.anchoredPosition, _endPoint.anchoredPosition);
        _arcLength = _arcDefinition.GetArcLength();
        
        UpdateSliderLength();

        _propagateDrag._onDrag += OnSliderValueChanged;
    }

    
    private void UpdateSliderLength()
    {
        _sLength = Mathf.Max(0, (_activePool.Count - 1) * _itemSpacing);
    }

    public override void Sync()
    {
        base.Sync();
        UpdateSliderLength();
    }

    public override void RefreshPivotsAsync()
    {
        CalculateCurvedPositions();
        base.RefreshPivotsAsync();
    }

    public override void RefreshPivots()
    {
        CalculateCurvedPositions();
        base.RefreshPivots();
    }

    private float MapSToT(float s, int itemIndex, int itemCount)
    {
        if (itemCount == 1)
            return 0.5f;
        float itemS = itemIndex * _itemSpacing;
        float viewportS = s;
        float delta = itemS - viewportS;
        float n_t = 0.5f + delta / _arcLength;
        return Mathf.Clamp01(n_t);
    }

    private void CalculateCurvedPositions()
    {
        int count = _activePool.Count;
        if (count == 0) return;
        
        for (int i = 0; i < count; i++)
        {
            DelegatingView delegatingView = _activePool[i] as DelegatingView;

            float t = MapSToT(_s, i, count);
            
            // 获取圆弧上的位置和旋转
            Vector2 position = _arcDefinition.EvaluatePoint(t);
            float rotation = _arcDefinition.EvaluateAngle(t);
            
            // 设置DelegatingView的位置和旋转
            delegatingView.GetRect().anchoredPosition = position;
            delegatingView.GetRect().rotation = Quaternion.Euler(0, 0, rotation);
        }
    }

    public void OnSliderValueChanged(PointerEventData eventData)
    {
        var dS = -eventData.delta.x;
        _s += dS;
        _s = Mathf.Clamp(_s, 0, _sLength);
        
        RefreshPivots();
    }
}
