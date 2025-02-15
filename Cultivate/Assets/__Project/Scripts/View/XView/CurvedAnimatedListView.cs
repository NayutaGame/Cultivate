
using UnityEngine;
using UnityEngine.EventSystems;

public class CurvedAnimatedListView : AnimatedListView
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _middlePoint;
    [SerializeField] private Transform _endPoint;

    private float _sliderValue = 0f;

    [SerializeField] private PropagateDrag _propagateDrag;

    private ArcDefinition _arcDefinition;
    private float _arcLength;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _arcDefinition = ArcDefinition.FromThreePoints(_startPoint.position, _middlePoint.position, _endPoint.position);

        _arcLength = _arcDefinition.GetArcLength();
        _sliderValue = _arcLength / 2;

        _propagateDrag._onDrag += OnSliderValueChanged;
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

    private float MapSToT(float sliderValue, int itemIndex, int itemCount)
    {
        if (itemCount == 1)
            return 0.5f;
            
        float s = sliderValue / _arcLength;
        
        // s: 滑动值 [0,1]
        // itemIndex: 当前卡牌索引 [0 to itemCount-1]
        // itemCount: 总卡牌数

        // 1. 计算卡牌的相对位置 [0,1]
        float relativePosition = itemIndex / (float)(itemCount - 1);
        
        // 2. 计算偏移
        // - 当s=0时，第一张卡(index=0)应该在t=0.5
        // - 当s=1时，最后一张卡(index=itemCount-1)应该在t=0.5
        float offset = 0.5f - s;  // 加入缩放因子
        
        // 3. 计算最终的t值
        float t = relativePosition + offset;
        t = Mathf.Clamp01(t);
        
        return t;
    }

    private void CalculateCurvedPositions()
    {
        int count = _activePool.Count;
        if (count == 0) return;
        
        for (int i = 0; i < count; i++)
        {
            DelegatingView delegatingView = _activePool[i] as DelegatingView;

            float t = MapSToT(_sliderValue, i, count);
            
            // 获取圆弧上的位置和旋转
            Vector2 position = _arcDefinition.EvaluatePoint(t);
            float rotation = _arcDefinition.EvaluateAngle(t);
            
            // 设置DelegatingView的位置和旋转
            delegatingView.GetRect().position = position;
            delegatingView.GetRect().rotation = Quaternion.Euler(0, 0, rotation);
        }
    }

    public void OnSliderValueChanged(PointerEventData eventData)
    {
        var dS = -eventData.delta.x / _arcLength;
        _sliderValue += dS;
        _sliderValue = Mathf.Clamp(_sliderValue, 0, _arcLength);
        
        RefreshPivots();
    }
}
