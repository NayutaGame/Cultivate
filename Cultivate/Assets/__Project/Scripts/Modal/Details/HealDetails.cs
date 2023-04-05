using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class HealDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public bool Cancel;

    public HealDetails(StageEntity src, StageEntity tgt, int value)
    {
        Src = src;
        Tgt = tgt;
        Value = value;

        Cancel = false;
    }
}
