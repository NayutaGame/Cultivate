using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxTweenDescriptor : TweenDescriptor
{
    public EntitySlot Slot;
    public string Text;

    public VfxTweenDescriptor(EntitySlot slot, string text)
    {
        Slot = slot;
        Text = text;
    }
}
