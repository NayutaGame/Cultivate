
public interface ICellAdapter
{
    Cell AsCell();
    bool IsCellNode { get; }

    void Enter();
    void Exit();
    void ReceiveSignal(Signal signal);
}