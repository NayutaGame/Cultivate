using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewardDescriptor
{
    public abstract void Claim();

    public abstract string GetDescription();
}
