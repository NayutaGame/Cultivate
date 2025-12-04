
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    private int _targetItemCount;
    private bool _targetIsMutator;
    private SkillEntryQuery _toQuery;
    private RunCostDefinition _refreshCost;

    public ListModel<RunSkill> LeftBucketItems;
    public ListModel<SkillGhost> RightBucketItems;
    public ListModel<SkillGhost> BoardItems;

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
        _targetItemCount = targetItemCount;
        _targetIsMutator = targetIsMutator;
        _toQuery = toQuery;
        _refreshCost = refreshCost;

        LeftBucketItems = new();
        RightBucketItems = new();
        BoardItems = new();
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
        for (int i = 0; i < _targetItemCount; i++)
        {
            GainSkillBuilder b = new();
            if (!_targetIsMutator)
            {
                b.Draw(_toQuery, RunManager.Instance.Environment.JingJie, consume: false);
            }
            else
            {
                b.DrawMutator(JingJie.HuaShen);
            }
            BoardItems.Add(SkillGhost.FromGainingSkill(b.GainingSkills[0]));
        }
    }

    public int GetValueDiff()
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

        return leftValue - rightValue;
    }

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
    
    private void ExchangeProcedure()
    {
        if (GetValueDiff() < 0)
            return;

        GainSkillBuilder b = new();
        if (!_targetIsMutator)
        {
            for (int i = 0; i < RightBucketItems.Count(); i++)
            {
                b.Draw(SkillEntryQuery.FromSkillGhost(RightBucketItems[i]), RightBucketItems[i].GetJingJie());
            }
        }
        else
        {
            for (int i = 0; i < RightBucketItems.Count(); i++)
            {
                b.Pick(RightBucketItems[i].Clone());
            }
        }
        b.Execute();
        
        // remove left items
        // staging

        // ExchangeSkillDetails details = new ExchangeSkillDetails();
        // RunManager.Instance.Environment.ExchangeSkillProcedure(details);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            // withdraw left items
            return null;
        }

        return this;
    }

    #region MoveSkillGhostRelated

    public void FromBoardToRightBucketProcedure(int fromIndex, int toIndex)
    {
        // SkillGhost fromSkill = BoardItems[fromIndex];
        // RunSkill toSkill = d.SkillSlot.Skill;
        //
        // if (toSkill == null)
        // {
        //     Hand.Remove(fromSkill);
        //     d.SkillSlot.Skill = fromSkill;
        // }
        // else
        // {
        //     Hand.Replace(fromSkill, toSkill);
        //     d.SkillSlot.Skill = fromSkill;
        // }
        //
        // EquipNeuron.Invoke(d);
        // SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    public void FromRightBucketToBoardProcedure(int fromIndex, int toIndex)
    {
        
    }

    #endregion
}
