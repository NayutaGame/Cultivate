using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class AttackDetails
{
    public StringBuilder Seq;
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Cancel;

    public AttackDetails(StringBuilder seq, StageEntity src, StageEntity tgt, int value)
    {
        Seq = seq;
        Src = src;
        Tgt = tgt;
        Value = value;
        Cancel = false;
    }
}
