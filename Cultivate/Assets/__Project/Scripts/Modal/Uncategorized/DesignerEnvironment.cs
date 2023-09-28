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
        RunEnvironment env = RunManager.Instance.Battle;
        env.Map.JingJie = JingJie.LianQi;
        env.AddXiuWei(50);
        env.ForceDrawSkills(jingJie: JingJie.LianQi, count: 5);
    }

    public static void Custom()
    {
        RunEnvironment env = RunManager.Instance.Battle;
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
