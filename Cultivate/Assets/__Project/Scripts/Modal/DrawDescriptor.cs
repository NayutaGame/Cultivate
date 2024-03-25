
public class DrawDescriptor
{
    public enum NodeType
    {
        Rest,
        Shop,
        Adventure,
        Ascension,
        Battle,
        Boss,
    }

    private NodeType _nodeType;
    public NodeType GetNodeType() => _nodeType;
    private NodeEntry _priority;

    public DrawDescriptor(NodeType nodeType, NodeEntry priority = null)
    {
        _nodeType = nodeType;
        _priority = priority;
    }

    public void Draw(Map map, StepItem stepItem, int level, int step)
    {
        if (_priority != null)
        {
            SetStepItemFromPriority(map, stepItem, level, step);
            return;
        }

        SetStepItemFromPool(map, stepItem, level, step);
    }

    private void SetStepItemFromPriority(Map map, StepItem stepItem, int level, int step)
    {
        stepItem._nodes.Clear();
        stepItem._nodes.Add(new RunNode(_priority));
    }

    private void SetStepItemFromPool(Map map, StepItem stepItem, int level, int step)
    {
        stepItem._nodes.Clear();

        switch (_nodeType)
        {
            case NodeType.Rest:
                stepItem._nodes.Add(new RunNode("休息"));
                break;
            case NodeType.Shop:
                stepItem._nodes.Add(new RunNode("商店"));
                break;
            case NodeType.Adventure:
            case NodeType.Ascension:
                map.AdventurePool.TryPopItem(out NodeEntry entry, pred: e => e.CanCreate(map, step));
                stepItem._nodes.Add(new RunNode(entry));
                break;
            case NodeType.Battle:
            case NodeType.Boss:
                DrawEntityDetails d = new DrawEntityDetails(level, step);
                map.EntityPool.TryDrawEntity(out RunEntity entity, d);
                stepItem._nodes.Add(new BattleRunNode(entity));
                break;
        }
    }
}
