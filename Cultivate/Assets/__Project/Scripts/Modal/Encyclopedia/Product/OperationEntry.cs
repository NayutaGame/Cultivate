
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OperationEntry : Entry
{
    private string _description;
    public string Description;

    private int _cost;
    public int Cost => _cost;

    private List<ILock> _locks;
    public List<ILock> Locks => _locks;

    public OperationEntry(string name, string description, int cost, List<ILock> locks = null) : base(name)
    {
        _description = description;
        _cost = cost;
        _locks = locks ?? new();
    }

    public virtual bool IsClick => false;
    public virtual bool IsDrag => false;
}
