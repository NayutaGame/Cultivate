
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    public static readonly int MAX_BOARD_ITEMS = 4;
    
    private int _targetItemCount;
    private bool _targetIsMutator;
    private SkillEntryQuery _toQuery;
    private RunCostDefinition _refreshCost;

    public ListModel<RunSkill> LeftBucketItems;
    public ListModel<SkillGhost> RightBucketItems;
    public ListModel<BarterBoardSlot> BoardItems;

    public int Weight { get; private set; }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BarterCell)thisObject).GetGuideDescriptor() },
        { "LeftBucketItems",            thisObject => ((BarterCell)thisObject).LeftBucketItems },
        { "RightBucketItems",           thisObject => ((BarterCell)thisObject).RightBucketItems },
        { "BoardItems",                 thisObject => ((BarterCell)thisObject).BoardItems },
    };
    public override object Get(string s) => Accessor[s](this);
    private BarterCell(
        int targetItemCount,
        bool targetIsMutator,
        SkillEntryQuery toQuery,
        RunCostDefinition refreshCost)
    {
        _targetItemCount = targetItemCount.ClampUpper(MAX_BOARD_ITEMS);
        _targetIsMutator = targetIsMutator;
        _toQuery = toQuery;
        _refreshCost = refreshCost;

        LeftBucketItems = new();
        RightBucketItems = new();
        BoardItems = new();
        MAX_BOARD_ITEMS.Do(i => BoardItems.Add(new()));
        
        RunManager.Instance.Environment.FromHandToBarterNeuron.Add(UpdateWeight);
        RunManager.Instance.Environment.FromFieldToBarterNeuron.Add(UpdateWeight);
        RunManager.Instance.Environment.FromBarterToHandNeuron.Add(UpdateWeight);
        RunManager.Instance.Environment.FromBarterToFieldNeuron.Add(UpdateWeight);
        RunManager.Instance.Environment.FromBoardToRightBucketNeuron.Add(UpdateWeight);
        RunManager.Instance.Environment.FromRightBucketToBoardNeuron.Add(UpdateWeight);

        UpdateWeight();
    }

    ~BarterCell()
    {
        RunManager.Instance.Environment.FromHandToBarterNeuron.Remove(UpdateWeight);
        RunManager.Instance.Environment.FromFieldToBarterNeuron.Remove(UpdateWeight);
        RunManager.Instance.Environment.FromBarterToHandNeuron.Remove(UpdateWeight);
        RunManager.Instance.Environment.FromBarterToFieldNeuron.Remove(UpdateWeight);
        RunManager.Instance.Environment.FromBoardToRightBucketNeuron.Remove(UpdateWeight);
        RunManager.Instance.Environment.FromRightBucketToBoardNeuron.Remove(UpdateWeight);
    }

    public static BarterCell FromCount(int targetItemCount = 2)
        => new(targetItemCount, false, null, null);

    public static BarterCell FromFanXuMingYuanShop()
    {
        return new(
            targetItemCount: 4,
            targetIsMutator: true,
            toQuery: null,
            refreshCost: new SkillCostDefinition(RunSkillQuery.FromName("命石"), "需要1命石"));
    }

    public static BarterCell FromEverything(
        int targetItemCount,
        bool targetIsMutator,
        SkillEntryQuery toQuery,
        RunCostDefinition refreshCost)
        => new(targetItemCount, targetIsMutator, toQuery, refreshCost);

    private void RefreshItems()
    {
        MAX_BOARD_ITEMS.Do(i => BoardItems[i].SkillGhost = null);

        ClearRightBucketItemsProcedure();
        
        for (int i = 0; i < _targetItemCount; i++)
        {
            GainSkillBuilder b = new();
            if (!_targetIsMutator)
            {
                b.Draw(_toQuery, RunManager.Instance.Environment.Map.JingJie, consume: false);
            }
            else
            {
                b.DrawMutator(RunManager.Instance.Environment.Map.JingJie);
            }

            BoardItems[i].SkillGhost = SkillGhost.FromGainingSkill(b.GainingSkills[0]);
        }
        
        UpdateWeight();
    }

    private void UpdateWeight(FromHandToBarterDetails d) => UpdateWeight();
    private void UpdateWeight(FromFieldToBarterDetails d) => UpdateWeight();
    private void UpdateWeight(FromBarterToHandDetails d) => UpdateWeight();
    private void UpdateWeight(FromBarterToFieldDetails d) => UpdateWeight();
    private void UpdateWeight(FromBoardToRightBucketDetails d) => UpdateWeight();
    private void UpdateWeight(FromRightBucketToBoardDetails d) => UpdateWeight();

    private void UpdateWeight()
    {
        int leftValue = 0;
        foreach (RunSkill skill in LeftBucketItems)
        {
            leftValue += RoomDefinition.GetCardBasePriceFromJingJie(skill.GetJingJie());
        }

        int rightValue = 0;
        foreach (SkillGhost ghost in RightBucketItems)
        {
            rightValue += RoomDefinition.GetCardBasePriceFromJingJie(ghost.GetJingJie());
        }

        Weight = leftValue - rightValue;

        RunManager.Instance.Environment.BarterWeightIsUpdatedNeuron.Invoke(Weight);
    }

    public bool CanExchange()
        => Weight >= 0 && LeftBucketItems.Count() > 0 && RightBucketItems.Count() > 0;

    public bool RefreshItemsIsAllowed()
        => _refreshCost != null;

    public bool RefreshItemsIsAffordable()
        => _refreshCost?.Affordable() ?? false;

    public string GetRefreshItemsDescription()
        => $"刷新 {_refreshCost.GetDescription()}";

    public void RefreshItemsProcedure()
    {
        if (!RefreshItemsIsAffordable())
            return;

        _refreshCost.Consume();
        RefreshItems();
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);
        
        RefreshItems();
    }

    private void ClearRightBucketItemsProcedure()
    {
        int count = RightBucketItems.Count();
        for (int i = 0; i < count; i++)
            RightBucketItems.RemoveAt(0);

        RunManager.Instance.Environment.BarterClearRightBucketItemsNeuron.Invoke();
    }
    
    public void ExchangeProcedure()
    {
        if (!CanExchange())
            return;

        GainSkillBuilder b = new();
        for (int i = 0; i < RightBucketItems.Count(); i++)
        {
            b.Pick(RightBucketItems[i].Clone());
        }
        b.Execute();
        
        LeftBucketItems.Clear();
        RightBucketItems.Clear();
        
        UpdateWeight();

        RunManager.Instance.Environment.ExchangeSkillProcedure(new ExchangeSkillDetails());
    }

    public void BarterWithdrawLeftItemsProcedure()
    {
        int leftItemsCount = LeftBucketItems.Count();
        for (int i = 0; i < leftItemsCount; i++)
        {
            IDeckIndex from = DeckIndex.FromBarter(0);
            IDeckIndex to = new NextHandDeckIndexDefinition();
            
            RunManager.Instance.Environment.MoveSkillProcedure(from, to);
        }
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            BarterWithdrawLeftItemsProcedure();
            return null;
        }

        return this;
    }

    #region MoveSkillGhostRelated

    public void FromBoardToRightBucketProcedure(int fromIndex)
    {
        BarterBoardSlot slot = BoardItems[fromIndex];
        SkillGhost fromSkill = slot.SkillGhost;
        if (fromSkill == null)
            return;

        int toIndex = RightBucketItems.Count();
        
        RightBucketItems.Add(fromSkill);
        slot.SkillGhost = null;

        UpdateWeight();
        RunManager.Instance.Environment.FromBoardToRightBucketNeuron.Invoke(new(fromIndex, toIndex));
    }

    public void FromRightBucketToBoardProcedure(int fromIndex, int toIndex)
    {
        SkillGhost fromSkill = RightBucketItems[fromIndex];
        BarterBoardSlot toSlot = BoardItems[toIndex];

        if (toSlot.SkillGhost != null)
            return;
        
        RightBucketItems.Remove(fromSkill);
        toSlot.SkillGhost = fromSkill;

        UpdateWeight();
        RunManager.Instance.Environment.FromRightBucketToBoardNeuron.Invoke(new(fromIndex, toIndex));
    }

    #endregion
}
