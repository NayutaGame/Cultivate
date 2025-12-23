
using DG.Tweening;
using UnityEngine;

public class CLButtonPatternB : CLButton
{
    [SerializeField] private GameObject IdleContent;
    [SerializeField] private GameObject HoverContent;
    [SerializeField] private GameObject PressedContent;
    [SerializeField] private GameObject InactiveContent;
    
    protected override void JoinIdleTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(true));
        seq.AppendCallback(() => HoverContent.SetActive(false));
        seq.AppendCallback(() => PressedContent.SetActive(false));
        seq.AppendCallback(() => InactiveContent.SetActive(false));
    }

    protected override void JoinHoverTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(false));
        seq.AppendCallback(() => HoverContent.SetActive(true));
        seq.AppendCallback(() => PressedContent.SetActive(false));
        seq.AppendCallback(() => InactiveContent.SetActive(false));
    }

    protected override void JoinPressTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(false));
        seq.AppendCallback(() => HoverContent.SetActive(false));
        seq.AppendCallback(() => PressedContent.SetActive(true));
        seq.AppendCallback(() => InactiveContent.SetActive(false));
    }

    protected override void JoinInactiveTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(false));
        seq.AppendCallback(() => HoverContent.SetActive(false));
        seq.AppendCallback(() => PressedContent.SetActive(false));
        seq.AppendCallback(() => InactiveContent.SetActive(true));
    }
}