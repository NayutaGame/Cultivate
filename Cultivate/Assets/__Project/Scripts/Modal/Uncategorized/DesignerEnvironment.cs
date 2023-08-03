using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class DesignerEnvironment
{
    private static readonly int[] DrawCountPerJingJie = new int[] { 4, 4, 4, 4, 4, 4 };

    public static void EnterRun()
    {
        // Standard();
        Custom();
    }

    public static void Standard()
    {
        RunManager rm = RunManager.Instance;
        rm.Map.JingJie = JingJie.LianQi;
        rm.AddXiuWei(50);

        bool flag = rm.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: JingJie.LianQi, count: 5);
        if (!flag)
            throw new Exception();

        rm.Simulate.SkillInventory.AddSkills(skills);
        rm.Battle.SkillInventory.AddSkills(skills);
    }

    public static void Custom()
    {
        RunManager rm = RunManager.Instance;
        rm.Map.JingJie = JingJie.HuaShen;
        rm.Battle.Hero.SetJingJie(JingJie.HuaShen);

        foreach (var wuXing in WuXing.Traversal)
        {
            rm.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: JingJie.HuaShen, wuXing: wuXing, count: 3);
            rm.Battle.SkillInventory.AddSkills(skills);
        }

        rm.SkillPool.TryDrawSkill(out RunSkill skill, pred: e => e.Name == "少阴");
        rm.Battle.SkillInventory.AddSkill(skill);
    }

    public static void AddRewardForBattleRunNode(BattleRunNode battleRunNode)
    {
        JingJie j = RunManager.Instance.Map.JingJie;
        battleRunNode.AddReward(new DrawSkillRewardDescriptor("一些外功", jingJie: j, count: DrawCountPerJingJie[j]));
    }

    public static async Task DefaultStartTurn(StageEntity entity)
    {
        // StageManager.Instance.ArmorLoseProcedure(entity, entity.Armor);
    }
}
