
using System.Collections.Generic;
using FMOD;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;

public abstract class CellNode : Node, ILogicNode, ICellAdapter
{
    protected Cell _cell;

    public virtual NodePort PrevPort { get; }
    public virtual NodePort NextPort { get; }
    public virtual IEnumerable<ILogicNode> Prevs { get; }
    public virtual ILogicNode Next { get; }
    public abstract void Execute();
    
    public virtual Cell AsCell()
    {
        _cell ??= CreateInternalCell();
        return _cell;
    }
    
    protected abstract Cell CreateInternalCell();

    public bool IsCellNode => true;
    public void Enter()
    {
        _cell = AsCell();
        _cell._enter(_cell);
    }

    public void Exit()
    {
        _cell._exit(_cell);
    }

    public abstract void ReceiveSignal(Signal signal);
}