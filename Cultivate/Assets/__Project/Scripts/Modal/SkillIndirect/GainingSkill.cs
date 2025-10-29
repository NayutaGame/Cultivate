
public class GainingSkill
{
    private SkillEntry _entry;
    private JingJie _jingJie;
    private IDeckIndex _deckIndex;
    
    public SkillEntry GetEntry() => _entry;
    public JingJie GetJingJie() => _jingJie;
    public IDeckIndex GetDeckIndex() => _deckIndex;
    
    public GainingSkill(SkillEntry entry, JingJie jingJie, IDeckIndex deckIndex)
    {
        _entry = entry;
        _jingJie = jingJie;
        _deckIndex = deckIndex ?? new NextHandDeckIndexDefinition();
    }

    public void ReifyDeckIndex()
    {
        _deckIndex = _deckIndex.Reify();
    }
}