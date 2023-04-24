using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyDetails
{
    public JingJie JingJie;
    public bool AllowNormal;
    public bool AllowElite;
    public bool AllowBoss;
    public int Step;

    public CreateEnemyDetails(JingJie jingJie, int step = -1)
    {
        JingJie = jingJie;

        if (step == -1)
        {
            AllowNormal = true;
            AllowElite = true;
            AllowBoss = true;
            return;
        }

        Step = step;
        AllowNormal = step == 0;
        AllowElite = step == 1;
        AllowBoss = step == 2;
    }
}
