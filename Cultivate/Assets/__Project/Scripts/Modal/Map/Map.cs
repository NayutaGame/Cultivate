
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Serialization;

public class Map : Addressable
{
    private JingJie _jingJie;
    public JingJie JingJie => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;
    
    private StepItem _stepItem;
    
    public StepItem CurrStepItem
        => _stepItem;
    
    public int Step;
    public bool IsLastStep()
        => Step >= DrawDescriptors.Length - 1;
    
    public int Choice;
    public RunNode CurrNode
        => Choosing ? null : _stepItem._nodes[Choice];
    
    public DrawDescriptor[] DrawDescriptors;
    public EntityPool EntityPool;
    public Pool<NodeEntry> AdventurePool;
    public bool Choosing;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map()
    {
        _accessors = new()
        {
            { "StepItem", () => _stepItem },
        };

        _stepItem = new();
    }

    public void DrawNode()
        => DrawDescriptors[Step].Draw(this, _stepItem, JingJie, Step);

    public void CreateEntry()
        => CurrNode.Entry.Create(this, CurrNode, JingJie, Step);
}
