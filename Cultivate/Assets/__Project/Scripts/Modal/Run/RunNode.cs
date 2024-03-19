
using UnityEngine;

public class RunNode
{
    protected NodeEntry _entry;
    public NodeEntry Entry => _entry;

    protected SpriteEntry _spriteEntry;
    public Sprite Sprite => _spriteEntry.Sprite;

    private int _level;
    public int GetLevel() => _level;
    private int _step;
    public int GetStep() => _step;
    private int _choice;
    public int GetChoice() => _choice;

    public Vector2Int Position { get; private set; }
    public JingJie JingJie { get; private set; }

    private RunNodeState _state;
    public RunNodeState GetState() => _state;
    public void SetState(RunNodeState state) => _state = state;

    public virtual string GetTitle() => _entry.GetTitle();

    public RunNode(int level, int step, int choice, NodeEntry entry)
    {
        if (entry is RewardNodeEntry r)
        {
            _spriteEntry = r.SpriteEntry;
        }
        else if (entry is AdventureNodeEntry)
        {
            _spriteEntry = "奇遇";
        }

        _level = level;
        _step = step;
        _choice = choice;
        
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
}
