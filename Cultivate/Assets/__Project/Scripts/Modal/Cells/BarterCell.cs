
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class BarterCell : Cell
{
    private BarterInventory _inventory;
    public BarterInventory GetInventory() => _inventory;

    public BarterCell()
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Inventory",                GetInventory },
        };
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);

        RunEnvironment env = RunManager.Instance.Environment;

        Pool<SkillEntryDescriptor> pool = new Pool<SkillEntryDescriptor>();
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
            Assert.IsTrue(fromSkills[i].JingJie.HasValue);
            GainSkillBuilder b = new();
            b.Draw(descriptor);
            toSkills[i] = SkillEntryDescriptor.FromEntryJingJie(b.DrawnSkillEntries[0], fromSkills[i].JingJie.Value); // distinct, non consume
        }
        
        _inventory = new();
        for (int i = 0; i < fromSkills.Length; i++)
            _inventory.Add(new BarterItem(fromSkills[i], toSkills[i]));
    }

    public void ExchangeSkillProcedure(ExchangeSkillDetails d)
    {
        BarterItem barterItem = d.BarterItem;
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

        d.BarterItemIndex = _inventory.IndexOf(barterItem);
        _inventory.Remove(barterItem);

        d.DeckIndex = deckIndex;
        RunManager.Instance.Environment.ExchangeSkillProcedure(d);
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
