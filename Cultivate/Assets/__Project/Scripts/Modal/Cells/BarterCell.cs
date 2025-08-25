
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    private BarterInventory _inventory;
    public BarterInventory GetInventory() => _inventory;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BarterCell)thisObject).GetGuideDescriptor() },
        { "Inventory",                  thisObject => ((BarterCell)thisObject).GetInventory() },
    };
    public override object Get(string s) => Accessor[s](this);
    public BarterCell()
    {
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        RunEnvironment env = RunManager.Instance.Environment;

        FinitePool<SkillEntryDescriptor> pool = new FinitePool<SkillEntryDescriptor>();
        pool.Populate(env.TraversalDeckIndices()
            .Map(env.SkillFromDeckIndex)
            .FilterObj(skill => skill != null)
            .Map(SkillEntryDescriptor.FromRunSkill));
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), 2);

        SkillEntryDescriptor[] fromSkills = new SkillEntryDescriptor[count];
        for (int i = 0; i < fromSkills.Length; i++)
            pool.TryPopItem(out fromSkills[i]);

        SkillEntryDescriptor[] toSkills = new SkillEntryDescriptor[count];
        for (int i = 0; i < toSkills.Length; i++)
        {
            SkillEntryDescriptor descriptor = SkillEntryDescriptor.FromPredJingJie(
                skillEntry =>
                {
                    foreach(var s in fromSkills)
                        if (skillEntry == s.Entry)
                            return false;
                    return true;
                }, fromSkills[i].JingJie);
            Assert.IsTrue(fromSkills[i].JingJie != null);
            GainSkillBuilder b = new();
            b.Draw(descriptor);
            toSkills[i] = SkillEntryDescriptor.FromEntryJingJie(b.DrawnSkillEntries[0], fromSkills[i].JingJie); // distinct, non consume
        }
        
        _inventory = new();
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
