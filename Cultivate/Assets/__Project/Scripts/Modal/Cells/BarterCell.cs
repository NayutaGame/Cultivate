
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    private int _targetItemCount;
    private bool _targetIsMutator;
    private BarterInventory _inventory;
    private Predicate<RunSkill> _fromPred;
    private Predicate<SkillEntry> _toPred;

    private RunCostDefinition _refreshCost;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BarterCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((BarterCell)thisObject).GetInventory() },
    };
    public override object Get(string s) => Accessor[s](this);
    private BarterCell(int targetItemCount, bool targetIsMutator, Predicate<RunSkill> fromPred, Predicate<SkillEntry> toPred, RunCostDefinition refreshCost)
    {
        _targetItemCount = targetItemCount;
        _targetIsMutator = targetIsMutator;
        _inventory = new();
        _fromPred = fromPred;
        _toPred = toPred;
        _refreshCost = refreshCost;
    }

    public static BarterCell FromLiteral(int targetItemCount, Predicate<RunSkill> fromPred, Predicate<SkillEntry> toPred)
        => new(targetItemCount, false, fromPred, toPred, null);

    public static BarterCell FromCount(int targetItemCount = 2)
        => new(targetItemCount, false, null, null, null);

    public static BarterCell FromEverything(int targetItemCount, bool targetIsMutator, Predicate<RunSkill> fromPred, Predicate<SkillEntry> toPred, RunCostDefinition refreshCost)
        => new(targetItemCount, targetIsMutator, fromPred, toPred, refreshCost);

    public static BarterCell FromFanXuMingYuanShop()
    {
        return new(
            targetItemCount: 6,
            targetIsMutator: true,
            fromPred: runSkill => runSkill.GetEntry() == Encyclopedia.SkillCategory.FromName("命石"),
            toPred: null,
            refreshCost: new SkillCostDefinition(RunSkillQuery.FromName("命石"), "需要1命石"));
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

    private void PopulateInventory()
    {
        RunEnvironment env = RunManager.Instance.Environment;

        FinitePool<SkillReference> pool = new FinitePool<SkillReference>();
        pool.Populate(env.TraversalDeckIndices()
            .Map(env.SkillFromDeckIndex)
            .FilterObj(skill => skill != null)
            .FilterObj(skill => _fromPred == null || _fromPred(skill))
            .Map(SkillReference.FromRunSkill));
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), _targetItemCount);

        SkillReference[] fromSkills = new SkillReference[count];
        for (int i = 0; i < fromSkills.Length; i++)
        {
            pool.TryPopItem(out fromSkills[i]);
        }

        SkillReference[] toSkills = new SkillReference[count];
        for (int i = 0; i < toSkills.Length; i++)
        {
            List<Predicate<SkillEntry>> predicates = new List<Predicate<SkillEntry>>
            {
                skillEntry =>
                {
                    foreach(var s in fromSkills)
                        if (skillEntry == s.GetEntry())
                            return false;
                    return true;
                }
            };
            if (_toPred != null)
                predicates.Add(_toPred);
            SkillEntryQuery drawStrategy = SkillEntryQuery.FromPredicatesBaseJingJieBound(
                predicates, new(JingJie.LianQi, fromSkills[i].GetJingJie()));
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
    
    public BarterInventory GetInventory() => _inventory;

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
