
using UnityEngine;

public class ClickBattleGuide : Guide
{
    private string _comment;
    public string GetComment() => _comment;
    private Vector2? _position;
    public Vector2? GetPosition() => _position;

    public ClickBattleGuide(string comment, Vector2? position = null)
    {
        _comment = comment;
        _position = position;
    }

    public override bool ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is ClickCombatSignal)
        {
            SetComplete(panelDescriptor);
            return true;
        }

        return false;
    }

    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
