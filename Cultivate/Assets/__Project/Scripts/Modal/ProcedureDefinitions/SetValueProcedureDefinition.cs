
public class SetValueProcedureDefinition : ProcedureDefinition
{
    public string Key;
    public string Value;
    
    public SetValueProcedureDefinition(string key, string value)
    {
        Key = key;
        Value = value;
    }
    
    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        castResult[Key] = Value;
        return null;
    }
}