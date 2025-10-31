
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    private int _targetItemCount;
    private bool _targetIsMutator;
    private RunSkillQuery _fromQuery;
    private SkillEntryQuery _toQuery;
    private RunCostDefinition _refreshCost;
    
    private BarterInventory _inventory;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BarterCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((BarterCell)thisObject)._inventory },
    };
    public override object Get(string s) => Accessor[s](this);
    private BarterCell(
        int targetItemCount,
        bool targetIsMutator,
        RunSkillQuery fromQuery,
        SkillEntryQuery toQuery,
        RunCostDefinition refreshCost)
    {
        _targetItemCount = targetItemCount;
        _targetIsMutator = targetIsMutator;
        _fromQuery = fromQuery;
        _toQuery = toQuery;
        _refreshCost = refreshCost;
        
        _inventory = new();
    }

    public static BarterCell FromCount(int targetItemCount = 2)
        => new(targetItemCount, false, null, null, null);

    public static BarterCell FromFanXuMingYuanShop()
    {
        return new(
            targetItemCount: 6,
            targetIsMutator: true,
            fromQuery: RunSkillQuery.FromName("命石"), 
            toQuery: null,
            refreshCost: new SkillCostDefinition(RunSkillQuery.FromName("命石"), "需要1命石"));
    }

    public static BarterCell FromEverything(
        int targetItemCount,
        bool targetIsMutator,
        RunSkillQuery fromQuery,
        SkillEntryQuery toQuery,
        RunCostDefinition refreshCost)
        => new(targetItemCount, targetIsMutator, fromQuery, toQuery, refreshCost);

    private void PopulateInventory()
    {
        RunEnvironment env = RunManager.Instance.Environment;

        FinitePool<SkillReference> pool = new FinitePool<SkillReference>();
        pool.Populate(env.TraversalDeckIndices()
            .Map(env.SkillFromDeckIndex)
            .FilterObj(skill => skill != null)
            .FilterObj(skill => _fromQuery == null || _fromQuery.Matches(skill))
            .Map(SkillReference.FromRunSkill));
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), _targetItemCount);

        SkillReference[] fromSkills = new SkillReference[count];
        for (int i = 0; i < fromSkills.Length; i++)
        {
            pool.TryPopItem(out fromSkills[i]);
        }

        SkillReference[] toSkills = new SkillReference[count];

        Predicate<SkillEntry> differsFromFromSkills = skillEntry =>
        {
            foreach (var s in fromSkills)
                if (skillEntry == s.GetEntry())
                    return false;
            return true;
        };
        for (int i = 0; i < toSkills.Length; i++)
        {
            List<Predicate<SkillEntry>> predicates = new();
            predicates.Add(differsFromFromSkills);
            if (_toQuery != null)
                predicates.Add(_toQuery.Matches);
            SkillEntryQuery drawStrategy = SkillEntryQuery.FromPredicatesBaseJingJieBound(predicates, new(JingJie.LianQi, fromSkills[i].GetJingJie()));
            GainSkillBuilder b = new();
            if (!_targetIsMutator)
            {
                b.Draw(drawStrategy, fromSkills[i].GetJingJie(), consume: false);
            }
            else
            {
                b.DrawMutator(JingJie.HuaShen);
            }
            toSkills[i] = SkillReference.FromGainingSkill(b.GainingSkills[0]);
        }
        
        _inventory.Clear();
        for (int i = 0; i < fromSkills.Length; i++)
            _inventory.Add(new BarterItem(fromSkills[i], toSkills[i], Exchange));
    }

    public bool RefreshItemsIsAllowed()
        => _refreshCost != null;

    public bool RefreshItemsIsAffordable()
        => _refreshCost?.Affordable() ?? false;

    public string GetRefreshItemsDescription()
        => $"刷新 {_refreshCost.GetDescription()}";

    public void RefreshItems()
    {
        if (!RefreshItemsIsAffordable())
            return;

        _refreshCost.Consume();
        PopulateInventory();
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);
        
        PopulateInventory();
    }
    
    private void Exchange(BarterItem barterItem)
    {
        ExchangeSkillDetails details = new(barterItem);
        if (!_inventory.Contains(barterItem))
            return;
        
        bool success = RunManager.Instance.Environment.DeckIndexFromQuery(out DeckIndex deckIndex, RunSkillQuery.FromSkillReference(barterItem.FromSkill));
        if (!success)
            return;

        GainSkillBuilder b = new();
        if (!_targetIsMutator)
        {
            b.Draw(SkillEntryQuery.FromSkillReference(barterItem.ToSkill), barterItem.ToSkill.GetJingJie(), deckIndex);
        }
        else
        {
            b.Pick(barterItem.ToSkill.Clone(), deckIndex);
        }
        b.Execute();

        details.BarterItemIndex = _inventory.IndexOf(barterItem);
        _inventory.Remove(barterItem);

        details.DeckIndex = deckIndex;
        RunManager.Instance.Environment.ExchangeSkillProcedure(details);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            return null;
        }

        return this;
    }
}
