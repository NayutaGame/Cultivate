using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Gacha Cell", -6, true)]
public class GachaCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<float> PriceMultiplier = new(2f);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<EditorSkillEntryQuery>> DrawStrategies = new();
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorJingJie> PreferredJingJie = new(EditorJingJie.任意);
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> _next = new(self => self as ILogicNode);
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => _next;
    
    public override IEnumerable<ILogicNode> Prevs => prevs.Values;
    
    public override ILogicNode Next
    {
        get
        {
            var node = NextPort?.Connection?.Node as ILogicNode;
            while (node != null && node is ILogicConnector)
                node = node.Next;
            return node;
        }
    }
    
    public override void Execute()
    {
    }
    
    protected override Cell CreateInternalCell()
    {
        float priceMultiplier = PriceMultiplier.Value;
        
        List<SkillEntryQuery> drawStrategies = null;
        List<EditorSkillEntryQuery> editorQueries = DrawStrategies.Value;
        if (editorQueries != null && editorQueries.Count > 0)
        {
            drawStrategies = new List<SkillEntryQuery>(editorQueries.Count);
            for (int i = 0; i < editorQueries.Count; i++)
            {
                var eq = editorQueries[i];
                if (eq == null) continue;
                drawStrategies.Add(SkillEntryQuery.FromEditorQuery(eq));
            }
        }
        
        JingJie preferredJingJie = global::JingJie.FromEditor(PreferredJingJie.Value);
        
        return GachaCell.FromEverything(
            priceMultiplier: priceMultiplier,
            drawStrategies: drawStrategies,
            preferredJingJie: preferredJingJie
        );
    }
    
    public override bool ReceiveSignal(Signal signal)
    {
        ExitShopSignal exitShopSignal = signal as ExitShopSignal;
        if (exitShopSignal == null)
            return true;
        return false;
    }
}