using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class DamageDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Recursive;

    public Action<DamageDetails> Damaged;
    public Action<DamageDetails> Undamaged;

    public bool Cancel;

    public DamageDetails(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Action<DamageDetails> damaged = null,
        Action<DamageDetails> undamaged = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Recursive = recursive;

        Damaged = damaged;
        Undamaged = undamaged;

        Cancel = false;
    }
}
