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
        RunManager rm = RunManager.Instance;

        rm.Map.JingJie = JingJie.JinDan;

        bool flag = rm.SkillPool.TryDrawSkills(out List<RunSkill> skills, jingJie: JingJie.JinDan, count: 16);
        if (!flag)
            throw new Exception();

        rm.Battle.SkillInventory.AddRange(skills);
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
