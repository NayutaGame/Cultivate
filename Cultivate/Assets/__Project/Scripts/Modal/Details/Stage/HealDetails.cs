using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class HealDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;

    public HealDetails(StageEntity src, StageEntity tgt, int value, bool penetrate = false)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
    }
}
