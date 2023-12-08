
using System.Collections.Generic;

public class UnequipResult
{
    public bool Success;

    public bool IsRunSkill;
    public RunSkill RunSkill;
    public List<MechType> MechTypes;

    public UnequipResult(bool success)
    {
        Success = success;
    }
}
