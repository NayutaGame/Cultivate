
public class DrawDescriptor
{
    public enum NodeType
    {
        Rest,
        Shop,
        Adventure,
        Ascension,
        Normal,
        Elite,
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

    public void Draw(Map map, StepItem stepItem, JingJie jingJie, int step)
    {
        if (_priority != null)
        {
            SetStepItemFromPriority(map, stepItem, jingJie, step);
            return;
        }

        SetStepItemFromPool(map, stepItem, jingJie, step);
    }

    private void SetStepItemFromPriority(Map map, StepItem stepItem, JingJie jingJie, int step)
    {
        stepItem._nodes.Clear();
        stepItem._nodes.Add(new RunNode(_priority));
    }

    private void SetStepItemFromPool(Map map, StepItem stepItem, JingJie jingJie, int step)
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
                map.AdventurePool.TryPopItem(out NodeEntry entry, pred: e => e.CanCreate(map, jingJie, step));
                stepItem._nodes.Add(new RunNode(entry));
                break;
            case NodeType.Normal:
                DrawNormal(map, stepItem, jingJie);
                break;
            case NodeType.Elite:
                DrawElite(map, stepItem, jingJie);
                break;
            case NodeType.Boss:
                DrawBoss(map, stepItem, jingJie);
                break;
        }
    }

    private void DrawNormal(Map map, StepItem stepItem, JingJie jingJie)
    {
        DrawEntityDetails d = new DrawEntityDetails(jingJie, allowNormal: true);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        stepItem._nodes.Add(new BattleRunNode(entity));
    }

    private void DrawElite(Map map, StepItem stepItem, JingJie jingJie)
    {
        DrawEntityDetails d = new DrawEntityDetails(jingJie, allowElite: true);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        stepItem._nodes.Add(new BattleRunNode(entity));
    }

    private void DrawBoss(Map map, StepItem stepItem, JingJie jingJie)
    {
        DrawEntityDetails d = new DrawEntityDetails(jingJie, allowBoss: true);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        stepItem._nodes.Add(new BattleRunNode(entity));
    }
}
