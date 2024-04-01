
using UnityEngine;

public class RunNode
{
    protected NodeEntry _entry;
    public NodeEntry Entry => _entry;

    private RunNodeState _state;
    public RunNodeState GetState() => _state;
    public void SetState(RunNodeState state) => _state = state;

    public virtual Sprite GetSprite() => _entry.GetSprite();
    public virtual string GetName() => _entry.GetName();
    public virtual string GetDescription() => _entry.GetDescription();

    public RunNode(NodeEntry entry)
    {
        _entry = entry;
        _state = RunNodeState.Future;
    }

    public enum RunNodeState
    {
        Missed,
        Passed,
        Current,
        ToChoose,
        Future,
    }

    public PanelDescriptor CurrentPanel { get; private set; }

    public void ChangePanel(PanelDescriptor panel)
    {
        CurrentPanel?.Exit();
        CurrentPanel = panel;
        CurrentPanel?.Enter();
    }

    public void Finish()
    {
        ChangePanel(null);
        SetState(RunNodeState.Passed);
    }
}
