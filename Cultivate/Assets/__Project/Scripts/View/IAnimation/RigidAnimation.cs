
using DG.Tweening;
using UnityEngine;

public class RigidAnimation : IAnimation
{
    private RectTransform Slot;
    private RectTransform Content;
    
    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private Vector3 StartScale;

    private Vector3 Position;
    private Quaternion Rotation;
    private Vector3 Scale;
    
    private Ease Ease;
    private float Duration;

    private RigidAnimation(
        RectTransform slot,
        RectTransform content,
        Vector3 position,
        Quaternion rotation,
        Vector3 scale,
        Ease ease = Ease.OutQuad,
        float duration = 0.15f)
    {
        Slot = slot;
        Content = content;
        
        StartPosition = content.position;
        StartRotation = content.rotation;
        StartScale = content.localScale;

        Position = position;
        Rotation = rotation;
        Scale = scale;
        
        Ease = ease;
        Duration = duration;
    }

    public static RigidAnimation FromFollow(RectTransform slot, RectTransform content)
    {
        return new RigidAnimation(
            slot,
            content,
            Vector3.zero,
            Quaternion.identity,
            Vector3.one,
            Ease.OutQuad,
            0.15f);
    }

    public static RigidAnimation FromSlotOffset(RectTransform slot, RectTransform content, SlotOffset slotOffset)
    {
        return new RigidAnimation(
            slot,
            content,
            slotOffset.Position,
            slotOffset.Rotation,
            slotOffset.Scale,
            Ease.OutQuad,
            0.15f);
    }

    public static RigidAnimation FromPosition(RectTransform slot, RectTransform content, Vector3 position, Ease ease = Ease.OutQuad, float duration = 0.15f)
    {
        return new RigidAnimation(
            slot,
            content,
            position,
            Quaternion.identity,
            Vector3.one,
            ease,
            duration);
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease);
    }

    public void AppendHandle(Sequence seq)
    {
        seq.Append(GetHandle());
    }

    public void SetProgress(float t)
    {
        Content.position = Vector3.Lerp(StartPosition, Slot.position + Position, t);
        Content.rotation = Quaternion.Slerp(StartRotation, Rotation, t);
        Content.localScale = Vector3.Lerp(StartScale, Scale, t);
    }
}