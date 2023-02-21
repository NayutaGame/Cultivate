using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ArmorDetails
{
    public StringBuilder Seq;
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Cancel;

    public ArmorDetails(StringBuilder seq, StageEntity src, StageEntity tgt, int value)
    {
        Seq = seq;
        Src = src;
        Tgt = tgt;
        Value = value;
        Cancel = false;
    }
}
