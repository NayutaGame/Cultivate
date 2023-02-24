using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipEntry : Entry
{
    public JingJie JingJie { get; private set; }

    public ChipEntry(string name, JingJie jingJie) : base(name)
    {
        JingJie = jingJie;
    }

    public virtual bool IsNeiGong => false;
    public virtual bool IsWaiGong => false;
}
