
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CLButtonPatternA : CLButton
{
    [SerializeField] private Image ShowWhenInactive;
    [SerializeField] private Image HideWhenInactive;
    [SerializeField] private Image[] ShowWhenHover;

    protected override void JoinIdleTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(0, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(1, 0.15f));
        
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }

    protected override void JoinHoverTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(0, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(1, 0.15f));
        }
    }

    protected override void JoinPressTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(0.6f, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(1, 0.15f));
        }
    }

    protected override void JoinInactiveTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(1, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(0, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }
}