
using System;
using PuppyDragon.uNody;
using UnityEngine;

public abstract class CompareNode<T> : PredNode where T : IComparable
{
    public enum Method { LT, LE, EQ, GE, GT, NE }
    
    [NodeEnum(true)] [SerializeField]
    private Method Operation;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedAny)] [SerializeField]
    private InputPort<T> Lhs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.InheritedAny)] [SerializeField]
    private InputPort<T> Rhs;

    public override bool GetResult()
    {
        int value = Lhs.Value.CompareTo(Rhs.Value);
        return Operation switch
        {
            Method.LT => value < 0,
            Method.LE => value <= 0,
            Method.EQ => value == 0,
            Method.GE => value >= 0,
            Method.GT => value > 0,
            Method.NE => value != 0,
            _ => throw new InvalidCastException(),
        };
    }
}