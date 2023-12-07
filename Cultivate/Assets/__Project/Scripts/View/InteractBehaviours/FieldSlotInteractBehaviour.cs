
using UnityEngine;

public class FieldSlotInteractBehaviour : DeckSkillInteractBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    public override void SetOpaque(bool value)
    {
        CanvasGroup.alpha = value ? 1 : 0;
    }

    protected override Address GetSkillAddress()
        => AddressBehaviour.GetAddress().Append(".Skill");
}
