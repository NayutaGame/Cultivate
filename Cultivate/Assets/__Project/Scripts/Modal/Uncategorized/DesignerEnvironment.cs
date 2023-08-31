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
        rm.ForceDrawSkills(jingJie: JingJie.LianQi, count: 5);
    }

    public static void Custom()
    {
        RunManager rm = RunManager.Instance;
        rm.Map.JingJie = JingJie.YuanYing;
        rm.Battle.Hero.SetJingJie(JingJie.YuanYing);
    }

    public static async Task DefaultStartTurn(StageEntity owner, EventDetails eventDetails)
    {
        TurnDetails d = (TurnDetails)eventDetails;
        // if (d.Owner == owner)
        //     await owner.ArmorLoseSelfProcedure(owner.Armor);
    }
}
