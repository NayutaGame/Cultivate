using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTweenDescriptor : TweenDescriptor
{
    public BuffDetails BuffDetails;

    public BuffTweenDescriptor(BuffDetails buffDetails)
    {
        BuffDetails = buffDetails;
    }
}
