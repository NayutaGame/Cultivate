
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
        => Step >= StepDescriptors.Length - 1;
    
    public int Choice;
    public RunNode CurrNode
        => Choosing ? null : _stepItem._nodes[Choice];
    
    public StepDescriptor[] StepDescriptors;
    public StepDescriptor CurrStepDescriptor => StepDescriptors[Step];
    
    public EntityPool EntityPool;
    public Pool<NodeEntry> AdventurePool;
    public bool Choosing;

    public int Ladder;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map()
    {
        _accessors = new()
        {
            { "StepItem", () => _stepItem },
        };

        _stepItem = new();
        Ladder = 0;
    }

    public void DrawNode()
        => CurrStepDescriptor.Draw(this);

    public void CreateEntry()
        => CurrNode.Entry.Create(this);
}
