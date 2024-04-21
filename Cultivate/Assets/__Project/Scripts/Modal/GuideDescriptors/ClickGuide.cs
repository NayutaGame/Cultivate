
using UnityEngine;

public class ClickGuide : Guide
{
    private string _comment;
    public string GetComment() => _comment;
    private Vector2? _position;
    public Vector2? GetPosition() => _position;

    public ClickGuide(string comment, Vector2? position = null)
    {
        _comment = comment;
        _position = position;
    }

    public override bool ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is ClickGuideSignal)
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
