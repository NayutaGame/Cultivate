using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseHealthDetails : EventDetails
{
    public StageEntity Owner;
    public int Value;

    public LoseHealthDetails(StageEntity owner, int value)
    {
        Owner = owner;
        Value = value;
    }
}
