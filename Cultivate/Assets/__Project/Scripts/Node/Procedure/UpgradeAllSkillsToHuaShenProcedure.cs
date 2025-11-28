
[NodeWidth(300)]
[CreateNodeMenu("Procedure/Upgrade All Skills To HuaShen", -10, true)]
public class UpgradeAllSkillsToHuaShenProcedure : ProcedureNode
{
    public override void Procedure()
    {
        RunManager.Instance.Environment.UpgradeAllSkillsToHuaShenProcedure();
    }
}