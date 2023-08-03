using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDetails : StageEventDetails
{
    public StageEntity Owner;

    public StageDetails(StageEntity owner)
    {
        Owner = owner;
    }
}
