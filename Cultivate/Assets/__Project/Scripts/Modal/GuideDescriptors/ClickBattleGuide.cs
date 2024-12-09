
using UnityEngine;

public class ClickBattleGuide : Guide
{
    private Vector2? _position;
    public Vector2? GetPosition() => _position;

    public ClickBattleGuide(string comment, Vector2? position = null) : base(comment)
    {
        _position = position;
    }

    public override void ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is ClickCombatSignal)
            SetComplete(panelDescriptor);
    }

    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
