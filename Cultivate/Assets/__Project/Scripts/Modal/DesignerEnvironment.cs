using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DesignerEnvironment
{
    public static void EnterRun()
    {
        RunManager rm = RunManager.Instance;

        // rm.JingJie = JingJie.LianQi;
        rm.JingJie = JingJie.HuaShen;

        rm.TryPlug(rm.DrawChip("基础加灵力"), rm.DanTian[0, -4]);
        rm.TryPlug(rm.DrawChip("佯攻"), rm.DanTian[1, -4]);
        rm.TryPlug(rm.DrawChip("佯攻"), rm.DanTian[2, -4]);
        rm.TryPlug(rm.DrawChip("佯攻"), rm.DanTian[3, -4]);
        rm.TryPlug(rm.DrawChip("佯攻"), rm.DanTian[4, -4]);
        rm.TryPlug(rm.DrawChip("轻击"), rm.DanTian[1, -3]);
        rm.TryPlug(rm.DrawChip("轻击"), rm.DanTian[2, -3]);
        rm.TryPlug(rm.DrawChip("轻击"), rm.DanTian[3, -3]);
        rm.TryPlug(rm.DrawChip("重击"), rm.DanTian[1, -2]);
        rm.TryPlug(rm.DrawChip("重击"), rm.DanTian[2, -2]);
        rm.TryPlug(rm.DrawChip("超重击"), rm.DanTian[1, -1]);
        rm.TryPlug(rm.DrawChip("单手防御"), rm.DanTian[-3, -1]);
        rm.TryPlug(rm.DrawChip("单手防御"), rm.DanTian[-2, -1]);
        rm.TryPlug(rm.DrawChip("单手防御"), rm.DanTian[-1, -1]);
        rm.TryPlug(rm.DrawChip("双手防御"), rm.DanTian[-1, -2]);
        rm.TryPlug(rm.DrawChip("双手防御"), rm.DanTian[-2, -2]);
        rm.TryPlug(rm.DrawChip("全力防御"), rm.DanTian[-1, -3]);
    }

    public static void DefaultStartTurn(StageEntity entity)
    {
        StageManager.Instance.ArmorLoseProcedure(new StringBuilder(), entity, entity.Armor);
    }
}
