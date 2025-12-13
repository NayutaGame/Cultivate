
using DG.Tweening;
using UnityEngine;

public class LissajousAnimation : CLAnimation
{
    private RectTransform Slot;
    private RectTransform Content;
    
    private float XAmplitude;
    private float YAmplitude;
    private int XFrequency;
    private int YFrequency;

    private float Duration;
    
    // private float AttractTime;

    public LissajousAnimation(
        RectTransform content,
        RectTransform slot,
        float xAmplitude,
        float yAmplitude,
        int xFrequency,
        int yFrequency,
        float duration) : base(content)
    {
        Slot = slot;
        
        XAmplitude = xAmplitude;
        YAmplitude = yAmplitude;
        XFrequency = xFrequency;
        YFrequency = yFrequency;
        Duration = duration;
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease.Linear);
    }

    protected override void SetProgress(float t)
    {
        // 使用互质频率创建利萨茹曲线
        // t 从 0 到 1 完成一个完整周期，t=0 和 t=1 会回到同一个点
        float angle = t * 2f * Mathf.PI;
        
        float x = XAmplitude * Mathf.Sin(XFrequency * angle);
        float y = YAmplitude * Mathf.Cos(YFrequency * angle);
        
        Content.position = Slot.position + new Vector3(x, y, 0);
    }
}