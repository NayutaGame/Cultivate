using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ArmorDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Cancel;

    public ArmorDetails(StageEntity src, StageEntity tgt, int value)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Cancel = false;
    }
}
