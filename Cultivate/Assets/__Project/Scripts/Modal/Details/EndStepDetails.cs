using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EndStepDetails
{
    public StageEntity Caster;
    public StageWaiGong WaiGong;

    public EndStepDetails(StageEntity caster, StageWaiGong waiGong)
    {
        Caster = caster;
        WaiGong = waiGong;
    }
}
