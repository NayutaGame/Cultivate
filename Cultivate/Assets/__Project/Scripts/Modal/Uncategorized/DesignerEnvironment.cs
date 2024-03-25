
using System.Threading.Tasks;
using CLLibrary;

public class DesignerEnvironment
{
    public static DesignerConfig GetDesignerConfig()
        => new();

    public static async Task DefaultStartTurn(StageEntity owner, EventDetails eventDetails)
    {
        TurnDetails d = (TurnDetails)eventDetails;
        if (d.Owner != owner)
            return;
        // await owner.LoseArmorProcedure(owner.Armor);
    }
}
