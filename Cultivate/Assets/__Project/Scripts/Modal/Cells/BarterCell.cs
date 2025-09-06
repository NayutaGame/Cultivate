
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    private int _targetItemCount;
    private BarterInventory _inventory;
    private Predicate<RunSkill> _fromPred;
    private Predicate<SkillEntry> _toPred;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BarterCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((BarterCell)thisObject).GetInventory() },
    };
    public override object Get(string s) => Accessor[s](this);
    private BarterCell(int targetItemCount, Predicate<RunSkill> fromPred, Predicate<SkillEntry> toPred)
    {
        _targetItemCount = targetItemCount;
        _inventory = new();
        _fromPred = fromPred;
        _toPred = toPred;
    }

    public static BarterCell FromLiteral(int targetItemCount, Predicate<RunSkill> fromPred, Predicate<SkillEntry> toPred)
        => new(targetItemCount, fromPred, toPred);

    public static BarterCell FromCount(int targetItemCount = 2)
        => new(targetItemCount, null, null);

    public static BarterCell FromFanXuMingYuanShop()
    {
        return new(6, runSkill => runSkill.GetEntry() == Encyclopedia.SkillCategory.FromName("命石"), null);
    }
    
    public BarterInventory GetInventory() => _inventory;

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        RunEnvironment env = RunManager.Instance.Environment;

        FinitePool<SkillEntryDescriptor> pool = new FinitePool<SkillEntryDescriptor>();
        pool.Populate(env.TraversalDeckIndices()
            .Map(env.SkillFromDeckIndex)
            .FilterObj(skill => skill != null)
            .FilterObj(skill => _fromPred == null || _fromPred(skill))
            .Map(SkillEntryDescriptor.FromRunSkill));
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), _targetItemCount);

        SkillEntryDescriptor[] fromSkills = new SkillEntryDescriptor[count];
        for (int i = 0; i < fromSkills.Length; i++)
        {
            pool.TryPopItem(out fromSkills[i]);
        }

        SkillEntryDescriptor[] toSkills = new SkillEntryDescriptor[count];
        for (int i = 0; i < toSkills.Length; i++)
        {
            List<Predicate<SkillEntry>> predicates = new List<Predicate<SkillEntry>>
            {
                skillEntry =>
                {
                    foreach(var s in fromSkills)
                        if (skillEntry == s.Entry)
                            return false;
                    return true;
                }
            };
            if (_toPred != null)
                predicates.Add(_toPred);
            SkillEntryDescriptor descriptor = SkillEntryDescriptor.FromPredJingJie(
                predicates, fromSkills[i].JingJie);
            Assert.IsTrue(fromSkills[i].JingJie != null);
            GainSkillBuilder b = new();
            b.Draw(descriptor);
            toSkills[i] = SkillEntryDescriptor.FromEntryJingJie(b.DrawnSkillEntries[0], fromSkills[i].JingJie); // distinct, non consume
        }
        
        _inventory.Clear();
        for (int i = 0; i < fromSkills.Length; i++)
            _inventory.Add(new BarterItem(fromSkills[i], toSkills[i], Exchange));
    }
    
    private void Exchange(BarterItem barterItem)
    {
        ExchangeSkillDetails details = new(barterItem);
        if (!_inventory.Contains(barterItem))
            return;
        
        bool success = RunManager.Instance.Environment.DeckIndexFromDescriptor(out DeckIndex deckIndex, barterItem.FromSkill);
        if (!success)
            return;

        GainSkillBuilder b = new();
        b.Draw(barterItem.ToSkill);
        b.Create(barterItem.ToSkill.JingJie);
        b.RecordDeckIndex(deckIndex);
        b.Add();

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
