
using System.Text;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[NodeHeaderTint(typeof(string))]
[CreateNodeMenu("String/String Builder", -10, true)]
public class StringBuilderNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField] [Multiline(5)]
    private InputPort<string>[] Strings;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField]
    private OutputPort<string> Result = new(self => (self as StringBuilderNode).GetResult());

    private string GetResult()
    {
        StringBuilder sb = new();

        foreach (InputPort<string> str in Strings)
        {
            sb.Append(str.Value);
        }

        return sb.ToString();
    }
}