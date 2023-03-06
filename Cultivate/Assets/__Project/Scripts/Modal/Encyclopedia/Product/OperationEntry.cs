
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OperationEntry : Entry
{
    private string _description;
    public string Description;

    private int _cost;
    public int Cost => _cost;

    public OperationEntry(string name, string description, int cost) : base(name)
    {
        _description = description;
        _cost = cost;
    }

    public virtual bool IsClick => false;
    public virtual bool IsDrag => false;
}
