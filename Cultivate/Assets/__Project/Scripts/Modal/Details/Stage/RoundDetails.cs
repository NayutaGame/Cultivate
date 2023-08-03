using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundDetails : StageEventDetails
{
    public StageEntity Owner;

    public RoundDetails(StageEntity owner)
    {
        Owner = owner;
    }
}
