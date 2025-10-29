
using System.Collections.Generic;
using CLLibrary;

public class GainSkillBuilder
{
    private RunEnvironment _env;
    private SkillPool _skillPool;
    private MutatorPool _mutatorPool;

    private List<GainingSkill> _gainingSkills;
    public List<GainingSkill> GainingSkills => _gainingSkills;

    private List<SkillReference> _drawnSkills;
    public List<SkillReference> DrawnSkills => _drawnSkills;
    
    public GainSkillBuilder()
    {
        _env = RunManager.Instance.Environment;
        _skillPool = _env.SkillPool;
        _mutatorPool = _env.MutatorPool;

        _gainingSkills = new();

        _drawnSkills = new();
    }

    public void Pick(SkillReference skillReference, IDeckIndex preferredDeckIndex = null)
    {
        _gainingSkills.Add(new(skillReference.GetEntry(), skillReference.GetJingJie(), preferredDeckIndex));
    }

    public void Draw(SkillEntryQuery drawStrategy, JingJie jingJie, IDeckIndex deckIndex = null, bool consume = true)
    {
        _skillPool.TryPopItem(out SkillEntry skillEntry, drawStrategy.Matches);
        _skillPool.Shuffle();
        skillEntry ??= Encyclopedia.SkillCategory.Default();
        _gainingSkills.Add(new(skillEntry, jingJie, deckIndex));
        
        if (!consume && skillEntry != Encyclopedia.SkillCategory.Default())
            _skillPool.Populate(skillEntry);
    }

    public void Draw(List<SkillEntryQuery> drawStrategies, JingJie jingJie, bool filterEmpty = true, bool distinct = true, bool consume = true)
    {
        List<SkillEntry> toRet = new();
        
        for (int i = 0; i < drawStrategies.Count; i++)
        {
            SkillEntryQuery drawStrategy = drawStrategies[i];
            
            _skillPool.TryPopItem(out SkillEntry item, s =>
            {
                if (!drawStrategy.Matches(s))
                    return false;

                if (distinct && toRet.Contains(s))
                    return false;

                return true;
            });

            if (!filterEmpty || item != null)
                toRet.Add(item ?? Encyclopedia.SkillCategory.Default());
        }

        foreach (SkillEntry skillEntry in toRet)
            _gainingSkills.Add(new(skillEntry, jingJie, new NextHandDeckIndexDefinition()));

        if (!consume)
            _skillPool.Populate(toRet.FilterObj(s => s != Encyclopedia.SkillCategory.Default()));

        _skillPool.Shuffle();
    }

    public void DrawMutator(JingJie jingJie, IDeckIndex deckIndex = null)
    {
        bool success = _mutatorPool.Draw(out SkillEntry skillEntry, jingJie);
        if (success)
            _gainingSkills.Add(new(skillEntry, skillEntry.LowestJingJie, deckIndex));
    }

    public void Execute()
    {
        for (int i = 0; i < _gainingSkills.Count; i++)
        {
            GainingSkill gainingSkill = _gainingSkills[i];
            
            gainingSkill.ReifyDeckIndex();
            DeckIndex deckIndex = gainingSkill.GetDeckIndex().Reify();

            if (deckIndex.Region == SkillRegion.Field)
            {
                _env.Home.GetSlot(deckIndex.Index).Skill = RunSkill.FromGainingSkill(gainingSkill);
                continue;
            }

            if (deckIndex.Index < _env.Hand.Count())
            {
                _env.Hand.Replace(deckIndex.Index, RunSkill.FromGainingSkill(gainingSkill));
                continue;
            }
            
            _env.Hand.Add(RunSkill.FromGainingSkill(gainingSkill));
        }
    }

    public void Invoke()
    {
        _env.GainSkillNeuron.Invoke(this);
    }
}