using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyDetails
{
    public bool AllowNormal;
    public bool AllowElite;
    public bool AllowBoss;
    public CLLibrary.Range JingJieRange;

    public CreateEnemyDetails(CLLibrary.Range jingJieRange = null)
    {
        JingJieRange = jingJieRange ?? new CLLibrary.Range(0, 5);
    }
}
