using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTweenDescriptor : TweenDescriptor
{
    public AttackDetails AttackDetails;

    public AttackTweenDescriptor(AttackDetails attackDetails)
    {
        AttackDetails = attackDetails;
    }
}
