
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CLButtonPatternC : CLButton
{
    [SerializeField] private TMP_Text Text;
    [SerializeField] private Color IdleColor;
    [SerializeField] private Color HoverColor;
    [SerializeField] private Color PressedColor;
    [SerializeField] private Color InactiveColor;

    [SerializeField] private Image[] HoverImages;
    [SerializeField] private Image[] PressedImages;
    [SerializeField] private Image[] InactiveImages;
    
    protected override void JoinIdleTween(Sequence seq)
    {
        seq.Append(Text.DOColor(IdleColor, 0.15f));
            
        foreach (Image image in HoverImages)
            seq.Join(image.DOFade(0, 0.15f));
            
        foreach (Image image in PressedImages)
            seq.Join(image.DOFade(0, 0.15f));
            
        foreach (Image image in InactiveImages)
            seq.Join(image.DOFade(0, 0.15f));
    }

    protected override void JoinHoverTween(Sequence seq)
    {
        seq.Append(Text.DOColor(HoverColor, 0.15f));
            
        foreach (Image image in HoverImages)
            seq.Join(image.DOFade(1, 0.15f));
            
        foreach (Image image in PressedImages)
            seq.Join(image.DOFade(0, 0.15f));
            
        foreach (Image image in InactiveImages)
            seq.Join(image.DOFade(0, 0.15f));
    }

    protected override void JoinPressTween(Sequence seq)
    {
        seq.Append(Text.DOColor(PressedColor, 0.15f));
            
        foreach (Image image in HoverImages)
            seq.Join(image.DOFade(0, 0.15f));
            
        foreach (Image image in PressedImages)
            seq.Join(image.DOFade(1, 0.15f));
            
        foreach (Image image in InactiveImages)
            seq.Join(image.DOFade(0, 0.15f));
    }

    protected override void JoinInactiveTween(Sequence seq)
    {
        seq.Append(Text.DOColor(InactiveColor, 0.15f));
            
        foreach (Image image in HoverImages)
            seq.Join(image.DOFade(0, 0.15f));
            
        foreach (Image image in PressedImages)
            seq.Join(image.DOFade(0, 0.15f));
            
        foreach (Image image in InactiveImages)
            seq.Join(image.DOFade(1, 0.15f));
    }
}
