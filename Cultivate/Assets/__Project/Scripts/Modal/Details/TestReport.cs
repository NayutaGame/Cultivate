
using System;
using UnityEngine;

[Serializable]
public class TestReport
{
    [SerializeReference]
    public DateTime PhysicalTime;

    [SerializeReference]
    public TimeSpan PassedTime;
    
    [SerializeReference]
    public int CardSwapCount;

    [SerializeReference]
    public int MergeCount;
    
    [SerializeReference]
    public string TesterNote;

    [SerializeReference]
    public RunEntity Home;

    [SerializeReference]
    public JingJie JingJie;

    [SerializeReference]
    public SkillInventory Hand;

    [SerializeReference]
    public int Gold;

    protected TestReport()
    {
        CardSwapCount = 0;
        MergeCount = 0;
        TesterNote = string.Empty;
    }

    public void IncrementCardSwapCount(EquipDetails d)
        => CardSwapCount++;

    public void IncrementCardSwapCount(SwapDetails d)
        => CardSwapCount++;

    public void IncrementCardSwapCount(UnequipDetails d)
        => CardSwapCount++;

    public void IncrementMergeCount(MergeDetails d)
        => MergeCount++;

    public void TakeSnapshot(RunEnvironment env)
    {
        PhysicalTime = DateTime.Now;
        PassedTime = env.GetPassedTime();
        Home = env.Home.Clone();
        JingJie = env.JingJie;
        Hand = env.Hand.Clone();
        Gold = env.GetGold().Curr;
    }

    public virtual void OnEnter(RunEnvironment env)
    {
        env.EquipNeuron.Add(IncrementCardSwapCount);
        env.SwapNeuron.Add(IncrementCardSwapCount);
        env.UnequipNeuron.Add(IncrementCardSwapCount);
        env.MergeNeuron.Add(IncrementMergeCount);
    }

    public virtual void OnExit(RunEnvironment env)
    {
        env.EquipNeuron.Remove(IncrementCardSwapCount);
        env.SwapNeuron.Remove(IncrementCardSwapCount);
        env.UnequipNeuron.Remove(IncrementCardSwapCount);
        env.MergeNeuron.Remove(IncrementMergeCount);
        TakeSnapshot(env);
    }
}
