
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurvedListView : ListView
{
    [SerializeField] private ArcDefinitionView ArcProvider;
    [SerializeField] private PropagateDrag _propagateDrag;
    private ArcDefinition _arcDefinition;

    [SerializeField] [Range(3, 20)] private float WindowSize = 3;
    [SerializeField] private bool Cyclic;
    [SerializeField] [Range(-0.9f, 10f)] private float CurveIntensity;
    [SerializeField] [Range(1f, 100)] private float CursorSensitivity = 1f;
    [SerializeField] [Range(-180, 180)] private float RotationOffset;

    [SerializeField] private float _cursor;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _arcDefinition = ArcProvider.ArcDefinition;
        
        if (_propagateDrag != null)
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

    private float MapIToT(int itemIndex, int itemCount, float cursor, float windowSize, bool cyclic)
    {
        float offset = itemIndex - cursor;
        if (cyclic)
        {
            // offset = offset - itemCount = offset + itemCount = offset + n * itemCount
            // offset mod-itemCount space
            // pick closer to 0
            float mod = offset % itemCount;
            if (mod < 0f)
                mod += itemCount;

            if (mod > itemCount / 2f)
                mod -= itemCount;
            offset = mod;
        }
        float t = offset / windowSize;
        return Mathf.Clamp(t, -0.5f, 0.5f);
    }

    private float MapTToNonuniformT(float t, float intensity)
    {
        // t 的范围是 [-0.5, 0.5]
        // 将 t 映射到 [0, 1) 范围用于对称 power 映射
        float tf = t + 0.5f; // [-0.5, 0.5] -> [0, 1]
        
        float power = 1.0f + intensity;
        
        float tfMapped;
        if (tf < 0.5f)
        {
            tfMapped = 0.5f * Mathf.Pow(2.0f * tf, power);
        }
        else
        {
            tfMapped = 1.0f - 0.5f * Mathf.Pow(2.0f * (1.0f - tf), power);
        }
        
        // 将结果映射回 [-0.5, 0.5] 范围
        return tfMapped - 0.5f;
    }

    private void SetRectFromUniformT(SlotView slotView, float nonuniformT)
    {
        float t = nonuniformT + 0.5f;
        Vector2 position = _arcDefinition.EvaluatePoint(t);
        float rotation = _arcDefinition.EvaluateAngle(t);
        
        slotView.GetRect().anchoredPosition = position;
        slotView.GetRect().rotation = Quaternion.Euler(0, 0, rotation + RotationOffset);
    }

    private void CalculateCurvedPositions()
    {
        int count = _activePool.Count;
        if (count == 0) return;
        
        for (int i = 0; i < count; i++)
        {
            SlotView slotView = _activePool[i];
            float uniformT = MapIToT(i, count, _cursor, WindowSize, Cyclic);
            float nonuniformT = MapTToNonuniformT(uniformT, CurveIntensity);
            SetRectFromUniformT(slotView, nonuniformT);
        }
    }

    public void OnSliderValueChanged(PointerEventData eventData)
    {
        float dCursor = -eventData.delta.x * CursorSensitivity / 1000;
        SetCursorAsync(_cursor + dCursor);
    }

    private Tween _handle;

    public void SetCursor(float cursor)
    {
        if (_activePool == null || _activePool.Count == 0)
            return;

        _cursor = cursor;

        RefreshPivots();
    }

    public void SetCursorAsync(float cursor)
    {
        float newCursor;
        if (Cyclic)
        {
            newCursor = MapCursorToCloser(_cursor, cursor, _activePool.Count);
        }
        else
        {
            newCursor = Mathf.Clamp(cursor, 0, _activePool.Count - 1);
        }
        _handle?.Kill();
        _handle = DOTween.To(SetCursor, _cursor, newCursor, 0.15f);
        _handle.SetAutoKill().Restart();
    }

    private float MapCursorToCloser(float current, float target, int itemCount)
    {
        float itemCountF = itemCount;
        
        float currentLogical = Mathf.Repeat(current, itemCountF); // [0, itemCount)
        float targetLogical = Mathf.Repeat(target, itemCountF);   // [0, itemCount)
        
        float delta = targetLogical - currentLogical;
        
        if (delta > itemCountF * 0.5f)
            delta -= itemCountF;
        else if (delta < -itemCountF * 0.5f)
            delta += itemCountF;

        return current + delta;
    }
}
