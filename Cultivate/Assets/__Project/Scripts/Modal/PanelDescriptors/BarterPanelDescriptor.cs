
using CLLibrary;
using UnityEngine;

public class BarterPanelDescriptor : PanelDescriptor
{
    private BarterInventory _inventory;
    public BarterInventory GetInventory() => _inventory;

    public BarterPanelDescriptor()
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Inventory",                GetInventory },
        };
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();

        Pool<SkillEntryDescriptor> pool = new Pool<SkillEntryDescriptor>();
        pool.Populate(RunManager.Instance.Environment.TraversalDeckIndices()
            .Map(deckIndex => RunManager.Instance.Environment.GetSkillAtDeckIndex(deckIndex) as RunSkill)
            .FilterObj(skill => skill != null)
            .Map(SkillEntryDescriptor.FromRunSkill));
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), 6);

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
            toSkills[i] = SkillEntryDescriptor.FromEntry(RunManager.Instance.Environment.DrawSkill(descriptor)); // distinct, non consume
        }
        
        _inventory = new();
        for (int i = 0; i < fromSkills.Length; i++)
            _inventory.Add(new BarterItem(fromSkills[i], toSkills[i]));
    }

    public bool Exchange(BarterItem barterItem)
    {
        if (!_inventory.Contains(barterItem))
            return false;
        
        bool success = RunManager.Instance.Environment.FindDeckIndex(out DeckIndex deckIndex, barterItem.FromSkill);
        if (!success)
            return false;
        
        RunManager.Instance.Environment.AddSkillProcedure(barterItem.ToSkill.Entry, barterItem.ToSkill.JingJie, deckIndex);
        _inventory.Remove(barterItem);
        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is ExitShopSignal)
        {
            return null;
        }

        return this;
    }
}
