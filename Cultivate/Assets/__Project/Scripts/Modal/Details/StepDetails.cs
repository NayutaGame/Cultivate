using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StepDetails
{
    public StageEntity Caster;
    public StageWaiGong WaiGong;

    public StepDetails(StageEntity caster, StageWaiGong waiGong)
    {
        Caster = caster;
        WaiGong = waiGong;
    }
}
