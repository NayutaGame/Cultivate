
using UnityEngine;

public class RunNode
{
    public enum RunNodeState
    {
        Untouched,
        Missed,
        Passed,
        Current,
        ToChoose,
    }
    
    protected NodeEntry _entry;
    public NodeEntry Entry => _entry;

    private RunNodeState _state;
    public RunNodeState State
    {
        get => _state;
        set => _state = value;
    }

    private PanelDescriptor _panel;
    public PanelDescriptor Panel
    {
        get => _panel;
        set
        {
            _panel?.Exit();
            _panel = value;
            _panel?.Enter();
        }
    }

    public virtual Sprite GetSprite() => _entry.GetSprite();
    public virtual string GetName() => _entry.GetName();
    public virtual string GetDescription() => _entry.GetDescription();

    public RunNode(NodeEntry entry)
    {
        _entry = entry;
        _state = RunNodeState.Untouched;
    }

    public void Finish()
    {
        Panel = null;
        State = RunNodeState.Passed;
    }
}
