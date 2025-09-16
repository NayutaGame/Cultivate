
using System;
using System.Collections.Generic;

public class TrySetValueProcedureDefinition : ProcedureDefinition
{
    public string Key;
    public string Value;
    
    public TrySetValueProcedureDefinition(string key, string value)
    {
        Key = key;
        Value = value;
    }

    protected TrySetValueProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        string key,
        string value) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        Key = key;
        Value = value;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new TrySetValueProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            key: Key,
            value: Value
        );
    }
    
    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        TrySetValueProcedureDefinition pd = procedureDefinition as TrySetValueProcedureDefinition;
        if (!castResult.ContainsKey(pd.Key))
            castResult[pd.Key] = pd.Value;
    }
}