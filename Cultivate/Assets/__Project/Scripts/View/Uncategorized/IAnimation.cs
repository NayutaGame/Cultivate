
using System;
using DG.Tweening;
using UnityEngine;

public interface IAnimation
{
    Tween GetHandle();
    void SetProgress(float t);
}

public struct FollowAnimation : IAnimation
{
    private RectTransform Subject;
    private RectTransform Follow;

    private Vector2 StartPosition;
    private Vector3 StartScale;

    public FollowAnimation(RectTransform subject, RectTransform follow)
    {
        Subject = subject;
        Follow = follow;

        StartPosition = Subject.position;
        StartScale = Subject.localScale;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f).SetEase(Ease.OutQuad);
    }

    public void SetProgress(float t)
    {
        Subject.position = Vector2.Lerp(StartPosition, Follow.position, t);
        Subject.localScale = Vector3.Lerp(StartScale, Follow.localScale, t);
    }
}

public struct GoToConfigurationAnimation : IAnimation
{
    private RectTransform Parent;
    private RectTransform Subject;
    private Configuration Configuration;

    private Vector3 StartPosition;
    private Vector3 StartScale;

    public GoToConfigurationAnimation(RectTransform parent, RectTransform subject, Configuration configuration)
    {
        Subject = subject;
        Configuration = configuration;
        Parent = parent;
        
        StartPosition = subject.position;
        StartScale = subject.localScale;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f).SetEase(Ease.OutQuad);
    }

    public void SetProgress(float t)
    {
        Subject.position = Vector3.Lerp(StartPosition, Parent.position + Configuration.LocalPosition, t);
        Subject.localScale = Vector3.Lerp(StartScale, Configuration.LocalScale, t);
    }
}

public struct ShakeAnimation : IAnimation
{
    private RectTransform Parent;
    private RectTransform Subject;

    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private Vector3 StartScale;

    private Vector3 RotationAxis;

    public ShakeAnimation(RectTransform parent, RectTransform subject)
    {
        Subject = subject;
        Parent = parent;
        
        StartPosition = subject.position;
        StartRotation = subject.rotation;
        StartScale = subject.localScale;
        
        RotationAxis = subject.TransformDirection(new Vector3(1, 1, 8).normalized);
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 360, 0.2f).SetEase(Ease.Linear);
    }

    public void SetProgress(float angle)
    {
        // 1. 计算法线 n(t)：绕旋转轴旋转angle度
        Vector3 initialNormal = StartRotation * Vector3.forward;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, RotationAxis);
        Vector3 currentNormal = axisRotation * initialNormal;
        
        // 2. 计算θ(t)：绕法线的旋转角（相对于法线坐标系）
        float theta = -angle;
        
        // 3. 组合旋转：
        //    先绕轴旋转得到新的法线方向
        //    再绕新法线旋转θ，补偿翻滚
        Quaternion normalRotation = Quaternion.AngleAxis(theta, currentNormal);
        Quaternion finalRotation = normalRotation * axisRotation * StartRotation;
        
        // 4. 应用旋转（保持位置不变）
        Subject.rotation = finalRotation;
    }
}

public struct GuideAnimation : IAnimation
{
    private RectTransform Start;
    private RectTransform End;

    private RectTransform Subject;

    public GuideAnimation(RectTransform subject, RectTransform start, RectTransform end)
    {
        Subject = subject;
        Start = start;
        End = end;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 1.5f).SetEase(Ease.InOutQuad);
    }

    public void SetProgress(float t)
    {
        Subject.position = Vector3.Lerp(Start.position, End.position, t);
        Subject.localScale = Vector3.Lerp(Start.localScale, End.localScale, t);
    }
}

public struct FloatAnimation : IAnimation
{
    private readonly Action<float> _setActualFunc;
    private readonly Func<float> _getTargetFunc;
    private readonly float _startValue;

    public FloatAnimation(Action<float> setActualFunc, Func<float> getActualFunc, Func<float> getTargetFunc)
    {
        _setActualFunc = setActualFunc;
        _getTargetFunc = getTargetFunc;
        _startValue = getActualFunc();
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f).SetEase(Ease.OutQuad);
    }

    public void SetProgress(float t)
    {
        float actual = Mathf.Lerp(_startValue, _getTargetFunc(), t);
        _setActualFunc(actual);
    }
}
