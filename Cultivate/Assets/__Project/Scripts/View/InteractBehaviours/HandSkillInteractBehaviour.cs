
using UnityEngine;
using UnityEngine.UI;

public class HandSkillInteractBehaviour : DeckSkillInteractBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private CanvasGroup CanvasGroup;

    public override void SetRaycastable(bool value)
    {
        Image.raycastTarget = value;
    }

    public override void SetOpaque(bool value)
    {
        CanvasGroup.alpha = value ? 1 : 0;
    }

    protected override Address GetSkillAddress()
        => ComplexView.AddressBehaviour.GetAddress();
}
