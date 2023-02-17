using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class HealDetails
{
    public StringBuilder Seq;
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public HealDetails(StringBuilder seq, StageEntity src, StageEntity tgt, int value)
    {
        Seq = seq;
        Src = src;
        Tgt = tgt;
        Value = value;
    }
}
