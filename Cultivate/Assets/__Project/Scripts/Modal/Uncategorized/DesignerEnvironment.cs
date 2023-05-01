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

        rm.JingJie = JingJie.JinDan;

        16.Do(i => rm.TryDrawAcquired(JingJie.JinDan));
    }

    public static void AddRewardForBattleRunNode(BattleRunNode battleRunNode)
    {
        JingJie j = RunManager.Instance.JingJie;
        battleRunNode.AddReward(new DrawChipRewardDescriptor("一些练气外功", e => e is WaiGongEntry, j, DrawCountPerJingJie[j]));
    }

    public static async Task DefaultStartTurn(StageEntity entity)
    {
        // StageManager.Instance.ArmorLoseProcedure(entity, entity.Armor);
    }
}
