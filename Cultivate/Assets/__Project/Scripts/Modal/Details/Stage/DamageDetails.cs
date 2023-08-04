using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DamageDetails : StageEventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Recursive;

    public Func<DamageDetails, Task> Damaged;
    public Func<DamageDetails, Task> Undamaged;

    public DamageDetails(StageEntity src, StageEntity tgt, int value, bool recursive = true,
        Func<DamageDetails, Task> damaged = null,
        Func<DamageDetails, Task> undamaged = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Recursive = recursive;

        Damaged = damaged;
        Undamaged = undamaged;
    }
}
