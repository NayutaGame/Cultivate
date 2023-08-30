using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDetails : EventDetails
{
    public StageEntity Owner;

    public StageDetails(StageEntity owner)
    {
        Owner = owner;
    }
}
