using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EndStepDetails
{
    public StringBuilder Seq;
    public StageEntity Caster;
    public StageWaiGong WaiGong;

    public EndStepDetails(StringBuilder seq, StageEntity caster, StageWaiGong waiGong)
    {
        Seq = seq;
        Caster = caster;
        WaiGong = waiGong;
    }
}
