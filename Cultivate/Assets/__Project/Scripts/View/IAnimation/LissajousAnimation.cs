
using DG.Tweening;
using UnityEngine;

public class LissajousAnimation : FromCurrentAnimation
{
    private RectTransform Slot;
    
    private float XAmplitude;
    private float YAmplitude;
    private int XFrequency;
    private int YFrequency;
    private float XPhase;
    private float YPhase;

    private float Duration;

    private LissajousAnimation(
        RectTransform content,
        RectTransform slot,
        float xAmplitude,
        float yAmplitude,
        int xFrequency,
        int yFrequency,
        float xPhase,
        float yPhase,
        float duration) : base(content)
    {
        Slot = slot;
        
        XAmplitude = xAmplitude;
        YAmplitude = yAmplitude;
        XFrequency = xFrequency;
        YFrequency = yFrequency;
        Duration = duration;
        XPhase = xPhase;
        YPhase = yPhase;
    }

    public static LissajousAnimation FromBreathPattern(RectTransform content, RectTransform slot)
    {
        int yFrequency = Random.Range(3, 7);
        int duration = Random.Range(30, 45);
        float xPhase = Random.value;
        float yPhase = Random.value;
        return new LissajousAnimation(content, slot,
            0.04f,
            0.1f,
            2,
            yFrequency,
            xPhase,
            yPhase,
            duration);
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease.Linear);
    }

    protected override void SetProgress(float t)
    {
        TryRecordConfiguration();
        
        float blend = Mathf.Clamp01(t * 3);
        
        // 使用互质频率创建利萨茹曲线
        // t 从 0 到 1 完成一个完整周期，t=0 和 t=1 会回到同一个点
        float angleX = (t + XPhase) * 2f * Mathf.PI;
        float angleY = (t + YPhase) * 2f * Mathf.PI;
        
        float x = XAmplitude * Mathf.Sin(XFrequency * angleX);
        float y = YAmplitude * Mathf.Cos(YFrequency * angleY);
        
        Vector3 lissajousPosition = Slot.position + new Vector3(x, y, 0);
        
        Content.position = Vector3.Lerp(StartConfiguration.Position, lissajousPosition, blend);
    }
}