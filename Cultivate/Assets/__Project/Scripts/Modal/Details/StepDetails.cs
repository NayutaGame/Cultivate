using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StepDetails
{
    public StageEntity Owner;
    public StageWaiGong WaiGong;

    public StepDetails(StageEntity owner, StageWaiGong waiGong)
    {
        Owner = owner;
        WaiGong = waiGong;
    }
}
