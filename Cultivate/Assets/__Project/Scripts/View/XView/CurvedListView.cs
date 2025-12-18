
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurvedListView : ListView
{
    [SerializeField] private ArcDefinitionView ArcProvider;
    [SerializeField] private PropagateDrag _propagateDrag;
    private ArcDefinition _arcDefinition;

    [SerializeField] [Range(2, 20)] private float WindowSize = 2;
    [SerializeField] private bool Cyclic;
    [SerializeField] [Range(-0.9f, 10f)] private float CurveIntensity;
    [SerializeField] [Range(0.1f, 50)] private float CursorSensitivity = 1f;

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

    private float MapIToT(int itemIndex, float cursor, float windowSize, bool cyclic)
    {
        float offset = itemIndex - cursor;
        float t = 0.5f + offset / windowSize;
        
        if (!cyclic)
        {
            return Mathf.Clamp01(t);
        }
        
        return t;
    }

    private float MapTToNonuniformT(float t, float intensity)
    {
        float ti = Mathf.Floor(t);
        float tf = t - ti;
        
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
        
        return ti + tfMapped;
    }

    private void SetRectFromUniformT(SlotView slotView, float nonuniformT, bool cyclic)
    {
        float tForArc;
        if (cyclic)
        {
            tForArc = nonuniformT % 1.0f;
            if (tForArc < 0f)
            {
                tForArc += 1.0f;
            }
        }
        else
        {
            tForArc = Mathf.Clamp01(nonuniformT);
        }
        
        Vector2 position = _arcDefinition.EvaluatePoint(tForArc);
        float rotation = _arcDefinition.EvaluateAngle(tForArc);
        
        slotView.GetRect().anchoredPosition = position;
        slotView.GetRect().rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void CalculateCurvedPositions()
    {
        int count = _activePool.Count;
        if (count == 0) return;
        
        for (int i = 0; i < count; i++)
        {
            SlotView slotView = _activePool[i];
            float uniformT = MapIToT(i, _cursor, WindowSize, Cyclic);
            float nonuniformT = MapTToNonuniformT(uniformT, CurveIntensity);
            SetRectFromUniformT(slotView, nonuniformT, Cyclic);
        }
    }

    public void OnSliderValueChanged(PointerEventData eventData)
    {
        float dCursor = -eventData.delta.x / CursorSensitivity;
        SetCursorAsync(_cursor + dCursor);
    }

    private Tween _handle;

    public void SetCursor(float cursor)
    {
        if (_activePool == null || _activePool.Count == 0)
            return;

        int itemCount = _activePool.Count;
        
        if (Cyclic)
        {
            _cursor = cursor;
        }
        else
        {
            _cursor = Mathf.Clamp(cursor, 0f, itemCount - 1);
        }

        RefreshPivots();
    }

    public void SetCursorAsync(float cursor)
    {
        float mapped = MapCursorToCloser(_cursor, cursor, _activePool.Count);
        
        _handle?.Kill();
        _handle = DOTween.To(SetCursor, _cursor, mapped, 0.15f);
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
