
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

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

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        RunEnvironment env = RunManager.Instance.Environment;

        Pool<SkillEntryDescriptor> pool = new Pool<SkillEntryDescriptor>();
        pool.Populate(env.TraversalDeckIndices()
            .Map(env.GetSkillAtDeckIndex)
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
            toSkills[i] = SkillEntryDescriptor.FromEntryJingJie(env.InnerDrawSkill(descriptor), fromSkills[i].JingJie.Value); // distinct, non consume
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
        
        bool success = RunManager.Instance.Environment.FindDeckIndex(out DeckIndex deckIndex, barterItem.FromSkill);
        if (!success)
            return;

        DeckIndex? refDeckIndex = deckIndex;
        RunManager.Instance.Environment.InnerDrawCreateAdd(barterItem.ToSkill, ref refDeckIndex);

        d.BarterItemIndex = _inventory.IndexOf(barterItem);
        _inventory.Remove(barterItem);

        RunManager.Instance.Environment.ExchangeSkillProcedure(d);
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
