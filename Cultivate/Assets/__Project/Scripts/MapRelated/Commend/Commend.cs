
using System;
using UnityEngine;

[Serializable]
public abstract class Commend
{
    [SerializeField] private ProceedMode _proceedMode;

    public abstract void Execute();
    
    public ProceedMode GetProceedMode() => _proceedMode;
}