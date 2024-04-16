
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

        Pool<SkillDescriptor> pool = new Pool<SkillDescriptor>();
        pool.Populate(RunManager.Instance.Environment.TraversalDeckIndices()
            .Map(deckIndex => RunManager.Instance.Environment.GetSkillAtDeckIndex(deckIndex) as RunSkill)
            .FilterObj(skill => skill != null)
            .Map(SkillDescriptor.FromRunSkill));
        pool.Shuffle();

        int count = Mathf.Min(pool.Count(), 6);

        SkillDescriptor[] fromSkills = new SkillDescriptor[count];
        for (int i = 0; i < fromSkills.Length; i++)
            pool.TryPopItem(out fromSkills[i]);

        SkillDescriptor[] toSkills = new SkillDescriptor[count];
        for (int i = 0; i < toSkills.Length; i++)
        {
            SkillDescriptor descriptor = SkillDescriptor.FromPredJingJie(
                skillEntry =>
                {
                    foreach(var s in fromSkills)
                        if (skillEntry == s.Entry)
                            return false;
                    return true;
                }, fromSkills[i].JingJie);
            toSkills[i] = SkillDescriptor.FromEntry(RunManager.Instance.Environment.DrawSkill(descriptor)); // distinct, non consume
        }
        
        _inventory = new();
        for (int i = 0; i < fromSkills.Length; i++)
            _inventory.Add(new BarterItem(fromSkills[i], toSkills[i]));
    }

    public bool Exchange(BarterItem barterItem)
    {
        if (!_inventory.Contains(barterItem))
            return false;

        DeckIndex? optionalDeckIndex = RunManager.Instance.Environment.FindDeckIndex(barterItem.FromSkill);
        if (!optionalDeckIndex.HasValue)
            return false;

        DeckIndex deckIndex = optionalDeckIndex.Value;
        RunManager.Instance.Environment.AddSkillProcedure(barterItem.ToSkill.Entry, barterItem.ToSkill.JingJie, deckIndex);
        _inventory.Remove(barterItem);
        return true;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal) => null;
}
