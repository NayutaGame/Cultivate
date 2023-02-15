using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DamageDetails
{
    public Sequence Seq;
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public DamageDetails(Sequence seq, StageEntity src, StageEntity tgt, int value)
    {
        Seq = seq;
        Src = src;
        Tgt = tgt;
        Value = value;
    }
}
