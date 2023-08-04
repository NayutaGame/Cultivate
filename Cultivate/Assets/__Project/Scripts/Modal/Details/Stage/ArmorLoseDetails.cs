using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ArmorLoseDetails : StageEventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public ArmorLoseDetails(StageEntity src, StageEntity tgt, int value)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }
}
