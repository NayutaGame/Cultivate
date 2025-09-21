
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : Button4State
{
    [SerializeField] private Image IdleIcon;
    [SerializeField] private Image HoverIcon;
    [SerializeField] private TMP_Text PriceText;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void JoinHoverTween(Sequence seq)
    {
        base.JoinHoverTween(seq);
        PriceText.color = Color.white;
    }

    protected override void JoinIdleTween(Sequence seq)
    {
        base.JoinIdleTween(seq);
        PriceText.color = Color.white;
    }

    protected override void JoinInactiveTween(Sequence seq)
    {
        base.JoinInactiveTween(seq);
        PriceText.color = Color.red;
    }

    protected override void JoinPressTween(Sequence seq)
    {
        base.JoinPressTween(seq);
        PriceText.color = Color.white;
    }
}