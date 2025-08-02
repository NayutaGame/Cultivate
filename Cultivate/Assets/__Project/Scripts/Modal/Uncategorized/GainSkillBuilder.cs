
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public class GainSkillBuilder
{
    private RunEnvironment _env;
    private SkillPool _skillPool;
    private List<SkillEntry> _drawnSkillEntries;
    public List<SkillEntry> DrawnSkillEntries => _drawnSkillEntries;
    private List<RunSkill> _createdSkills;
    public List<RunSkill> CreatedSkills => _createdSkills;
    private List<IDeckIndex> _preferredDeckIndices;
    public List<IDeckIndex> PreferredDeckIndices => _preferredDeckIndices;
    
    public GainSkillBuilder()
    {
        _env = RunManager.Instance.Environment;
        _skillPool = _env.SkillPool;
        _drawnSkillEntries = new();
        _createdSkills = new();
        _preferredDeckIndices = new();
    }

    public void Pick(string skillName)
        => Pick(Encyclopedia.SkillCategory.FromName(skillName));
    public void Pick(SkillEntry skillEntry)
    {
        _drawnSkillEntries.Add(skillEntry);
    }

    public void Draw(SkillEntryDescriptor descriptor)
    {
        _skillPool.TryPopItem(out SkillEntry skillEntry, descriptor.Contains);
        _skillPool.Shuffle();
        skillEntry ??= Encyclopedia.SkillCategory.Default();
        _drawnSkillEntries.Add(skillEntry);
    }

    public void Draw(SkillEntryCollectionDescriptor d)
    {
        List<SkillEntry> toRet = new();
        
        for (int i = 0; i < d.Count; i++)
        {
            _skillPool.TryPopItem(out SkillEntry item, s =>
            {
                if (!d.Pred(s))
                    return false;

                if (d.Distinct && toRet.Contains(s))
                    return false;

                return true;
            });

            item ??= Encyclopedia.SkillCategory.Default();
            toRet.Add(item);
        }

        if (!d.Consume)
            _skillPool.Populate(toRet.FilterObj(s => s != Encyclopedia.SkillCategory.Default()));

        _skillPool.Shuffle();
        
        _drawnSkillEntries.AddRange(toRet);
    }
    
    public void Create(JingJie preferredJingJie = null)
    {
        int start = _createdSkills.Count;
        int count = _drawnSkillEntries.Count - _createdSkills.Count;
        for (int i = start; i < start + count; i++)
        {
            _createdSkills.Add(RunSkill.FromEntryJingJie(_drawnSkillEntries[i], preferredJingJie ?? JingJie.LianQi));
        }
    }

    public void SingleCreate(JingJie preferredJingJie = null)
    {
        int start = _createdSkills.Count;
        _createdSkills.Add(RunSkill.FromEntryJingJie(_drawnSkillEntries[start], preferredJingJie ?? JingJie.LianQi));
    }

    public void RecordDeckIndex(IDeckIndex deckIndex)
    {
        _preferredDeckIndices.Add(deckIndex);
    }

    public void Add()
    {
        for (int i = 0; i < _createdSkills.Count; i++)
        {
            DeckIndex deckIndex;
            if (i < _preferredDeckIndices.Count)
            {
                deckIndex = _preferredDeckIndices[i].Reify();
                _preferredDeckIndices[i] = deckIndex;
            }
            else
            {
                deckIndex = new NextHandDeckIndexDefinition().Reify();
                _preferredDeckIndices.Add(deckIndex);
            }

            if (deckIndex.InField)
            {
                _env.Home.GetSlot(deckIndex.Index).Skill = _createdSkills[i];
                continue;
            }

            if (deckIndex.Index < _env.Hand.Count())
            {
                _env.Hand.Replace(deckIndex.Index, _createdSkills[i]);
                continue;
            }
            
            _env.Hand.Add(_createdSkills[i]);
        }
    }

    public void Invoke()
    {
        _env.GainSkillNeuron.Invoke(this);
    }
}