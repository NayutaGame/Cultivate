
using System;
using System.Collections.Generic;

public class SetValueProcedureDefinition : ProcedureDefinition
{
    public string Key;
    public string Value;
    
    public SetValueProcedureDefinition(string key, string value)
    {
        Key = key;
        Value = value;
    }

    protected SetValueProcedureDefinition(
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
            clonedClosures[i] = Closures[i];

        return new SetValueProcedureDefinition(
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
        SetValueProcedureDefinition pd = procedureDefinition as SetValueProcedureDefinition;
        castResult[pd.Key] = pd.Value;
    }
}