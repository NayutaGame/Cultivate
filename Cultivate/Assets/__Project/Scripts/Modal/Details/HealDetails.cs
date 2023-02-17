using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HealDetails
{
    public Sequence Seq;
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public HealDetails(Sequence seq, StageEntity src, StageEntity tgt, int value)
    {
        Seq = seq;
        Src = src;
        Tgt = tgt;
        Value = value;
    }
}
