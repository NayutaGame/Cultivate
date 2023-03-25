using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DesignerEnvironment
{
    public static void EnterRun()
    {
        RunManager rm = RunManager.Instance;

        rm.JingJie = JingJie.LianQi;
    }

    public static void DefaultStartTurn(StageEntity entity)
    {
        // StageManager.Instance.ArmorLoseProcedure(entity, entity.Armor);
    }
}
