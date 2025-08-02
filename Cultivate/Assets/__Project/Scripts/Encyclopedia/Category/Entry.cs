
using System;
using UnityEngine;

public abstract class Entry
{
    [SerializeField] private string _id;
    [NonSerialized] private string _name;
    
    public string GetId() => _id;
    public string GetName() => _name;

    public Entry(string id, string name)
    {
        _id = id;
        _name = name;
    }

    public virtual void Init()
    {
        
    }
}
