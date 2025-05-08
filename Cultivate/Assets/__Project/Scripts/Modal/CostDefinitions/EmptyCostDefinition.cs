
using System;

public class EmptyCostDefinition : CostDefinition
{
    public EmptyCostDefinition() : base(0) { }

    public EmptyCostDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Func<CostDefinition, ResultDict, Description> getDescription,
        int value, StageClosure[] closures) :
        base(preCondDefinition, postCondDefinition, getDescription, value, closures) { }

    public override CostDefinition Clone()
    {
        StageClosure[] clonedClosures = new StageClosure[Closures.Length];
        for (int i = 0; i < Closures.Length; i++)
            clonedClosures[i] = Closures[i];

        return new EmptyCostDefinition(
            PreCondDefinition.Clone(),
            PostCondDefinition.Clone(),
            _getDescription,
            Value,
            clonedClosures
        );
    }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Empty, CostState.Normal, Value);
}