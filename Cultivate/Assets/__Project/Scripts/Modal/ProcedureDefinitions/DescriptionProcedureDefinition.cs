
using System;
using System.Collections.Generic;

public class DescriptionProcedureDefinition : ProcedureDefinition
{
    public DescriptionProcedureDefinition(
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription)
    {
        _getDescription = getDescription;
    }

    protected DescriptionProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new DescriptionProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures
        );
    }
    
    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        DescriptionProcedureDefinition pd = procedureDefinition as DescriptionProcedureDefinition;
        
        description.Sb.Append(pd.PostCondDefinition.Description);
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                description.AppendSoftReturn();
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}