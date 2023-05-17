using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTweenDescriptor : TweenDescriptor
{
    public HealDetails HealDetails;

    public HealTweenDescriptor(HealDetails healDetails)
    {
        HealDetails = healDetails;
    }
}
