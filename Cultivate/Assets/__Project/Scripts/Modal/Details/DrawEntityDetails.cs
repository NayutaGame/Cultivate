using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEntityDetails
{
    public JingJie JingJie;
    public bool AllowNormal;
    public bool AllowElite;
    public bool AllowBoss;

    public DrawEntityDetails(JingJie jingJie, bool allowNormal = false, bool allowElite = false, bool allowBoss = false)
    {
        JingJie = jingJie;
        AllowNormal = allowNormal;
        AllowElite = allowElite;
        AllowBoss = allowBoss;
    }
}
