
using System.Threading.Tasks;

public class DesignerEnvironment
{
    public static RunConfig GetConfig()
    {
        return new RunConfig(Standard);
        // return new RunConfig(Custom);
    }

    private static void Standard(RunEnvironment env)
    {
        env.Map.JingJie = JingJie.LianQi;
        env.AddXiuWei(50);
        env.ForceDrawSkills(jingJie: JingJie.LianQi, count: 5);
    }

    private static void Custom(RunEnvironment env)
    {
        env.Map.JingJie = JingJie.HuaShen;
        env.Home.SetJingJie(JingJie.HuaShen);
        env.ForceDrawSkills(jingJie: JingJie.HuaShen, count: 12);
    }

    public static async Task DefaultStartTurn(StageEntity owner, EventDetails eventDetails)
    {
        TurnDetails d = (TurnDetails)eventDetails;
        // if (d.Owner == owner)
        //     await owner.ArmorLoseSelfProcedure(owner.Armor);
    }
}
