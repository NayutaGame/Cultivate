
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RunEntity : Addressable, IEntity, ISerializationCallbackReceiver
{
    public Neuron EnvironmentChangedNeuron;

    public static readonly int[] BaseHealthFromJingJie = new int[] { 40, 100, 180, 280, 400, 400 };
    
    public static readonly string NORMAL_KEY = "Normal";
    public static readonly string SMIRK_KEY = "Smirk";
    public static readonly string AFRAID_KEY = "Afraid";

    [SerializeField] private MingYuan _mingYuan;
    public MingYuan MingYuan => _mingYuan;

    [SerializeField] private int _baseHealth;
    public int GetBaseHealth() => _baseHealth;
    public void SetBaseHealth(int health) => _baseHealth = health;

    [SerializeField] [OptionalField(VersionAdded = 2)] private int _dHealth;
    public int GetDHealth() => _dHealth;
    public void SetDHealth(int dHealth) => _dHealth = dHealth;
    public void SetHealthByModifyingDHealth(int finalHealth) => _dHealth = finalHealth - _baseHealth;

    public int GetFinalHealth() => _baseHealth + _dHealth;
    public BoundedInt GetFinalHealthBounded() => new(GetFinalHealth());

    #region Only For Editable

    [SerializeField] private int _ladder;
    public int GetLadder() => _ladder;
    public void SetLadder(int value) => _ladder = value;

    [SerializeField] [OptionalField(VersionAdded = 3)] private bool _inPool;
    public bool IsInPool() => _inPool;
    public void SetInPool(bool value) => _inPool = value;

    // obsolete
    [SerializeField] private bool _isNormal;
    [SerializeField] private bool _isElite;
    [SerializeField] private bool _isBoss;

    #endregion

    [SerializeField] private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;

    [SerializeField] private int _slotCount;
    public int GetSlotCount() => _slotCount;
    public void SetSlotCount(int slotCount)
    {
        _slotCount = slotCount;

        _slots.Count().Do(i =>
        {
            _slots[i].Hidden = i >= _slotCount;
        });

        _filteredSlots?.Refresh();
        
        EnvironmentChangedNeuron.Invoke();
    }
    
    public void SetSlotCountFromJingJie(JingJie jingJie)
    {
        SetSlotCount(RunManager.SlotCountFromJingJie[jingJie]);
    }

    [SerializeReference] private SlotListModel _smirkAgainstSlots;
    [SerializeReference] private SlotListModel _afraidAgainstSlots;
    
    public string GetReactionKeyFromSkill(RunSkill skill)
    {
        int? smirkIdx = _smirkAgainstSlots.Traversal().FirstIdx(s => s.Skill?.GetEntry() == skill.GetEntry());
        if (smirkIdx.HasValue)
            return SMIRK_KEY;
        
        int? afraidIdx = _afraidAgainstSlots.Traversal().FirstIdx(s => s.Skill?.GetEntry() == skill.GetEntry());
        if (afraidIdx.HasValue)
            return AFRAID_KEY;
        
        return NORMAL_KEY;
    }

    [SerializeReference] private SlotListModel _slots;
    [NonSerialized] private FilteredListModel<SkillSlot> _filteredSlots;

    public SkillSlot GetSlot(int i)
        => _slots[i];

    public IEnumerable<SkillSlot> TraversalCurrentSlots()
    {
        for (int i = 0; i < _slotCount; i++)
            yield return _slots[i];
    }

    public bool CanRemoveSkill(RunSkill skill)
    {
        SkillSlot slot = TraversalCurrentSlots().FirstObj(s => s.Skill == skill);
        return slot != null;
    }

    public bool TryRemoveSkill(RunSkill skill)
    {
        SkillSlot slot = TraversalCurrentSlots().FirstObj(s => s.Skill == skill);
        if (slot == null)
            return false;

        slot.Skill = null;
        return true;
    }

    public void PlacementProcedure()
    {
        PlacementDetails d = new(this);

        TraversalCurrentSlots().Do(slot =>
        {
            slot.PlacedSkill = null;
        });

        RunManager.Instance.Environment.ClosureDict.SendEvent(RunClosureDict.WILL_PLACEMENT, d);

        TraversalCurrentSlots().Do(slot =>
        {
            if (slot.PlacedSkill != null)
                return;

            if (slot.Skill != null)
            {
                slot.PlacedSkill = PlacedSkill.FromRunSkill(slot.Skill);
                return;
            }

            slot.PlacedSkill = PlacedSkill.FromEntryAndJingJie(d.OverridingSkillEntry, d.OverridingJingJie);
        });

        RunManager.Instance.Environment.ClosureDict.SendEvent(RunClosureDict.DID_PLACEMENT, d);
    }

    public void DepleteProcedure()
    {
        DepleteDetails d = new(this);

        RunManager.Instance.Environment.ClosureDict.SendEvent(RunClosureDict.WILL_DEPLETE, d);

        _slots.Traversal().Do(slot => slot.TryDeplete(d));

        RunManager.Instance.Environment.ClosureDict.SendEvent(RunClosureDict.DID_DEPLETE, d);
    }

    #region Formation

    [NonSerialized] private ListModel<RunFormation> _formations;
    public IEnumerable<RunFormation> TraversalFormations => _formations.Traversal();
    [NonSerialized] private FilteredListModel<RunFormation> _showingFormations;
    [NonSerialized] private FilteredListModel<RunFormation> _activeFormations;

    public void FormationProcedure()
    {
        RunFormationDetails d = new(this);

        _formations.Clear();

        RunManager.Instance.Environment.ClosureDict.SendEvent(RunClosureDict.WIL_FORMATION, d);

        _formations.AddRange(Encyclopedia.FormationCategory.Traversal
            .Map(e => RunFormation.From(e, e.GetProgress(this, d))));

        RunManager.Instance.Environment.ClosureDict.SendEvent(RunClosureDict.DID_FORMATION, d);

        _showingFormations.Refresh();
        _activeFormations.Refresh();
    }

    #endregion

    [SerializeField] private EntityEntry _entry;
    public EntityEntry GetEntry() => _entry;
    public void SetEntry(EntityEntry entry) => _entry = entry;

    public static RunEntity Default()
        => new();
    public static RunEntity Trainer()
        => new(jingJie: JingJie.LianQi, baseHealth: 1000000);
    public static RunEntity FromJingJieHealth(JingJie jingJie, int health)
        => new(jingJie: jingJie, baseHealth: health);
    public static RunEntity FromTemplate(RunEntity template)
        => new(entry: template._entry, mingYuan: template._mingYuan, jingJie: template._jingJie,
            baseHealth: template._baseHealth, slotCount: template._slotCount, slots: template._slots, template._smirkAgainstSlots, template._afraidAgainstSlots);
    public static RunEntity FromHardCoded(JingJie? jingJie = null,
        int? baseHealth = null, int? slotCount = null, RunSkill[] skills = null)
        => new(jingJie: jingJie, baseHealth: baseHealth, slotCount: slotCount, slots: skills != null ? SlotListModel.FromSkills(skills) : null);

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEntity(EntityEntry entry = null, MingYuan mingYuan = null, JingJie? jingJie = null,
        int? baseHealth = null, int? slotCount = null, SlotListModel slots = null, SlotListModel smirkAgainstSlots = null, SlotListModel afraidAgainstSlots = null)
    {
        _accessors = new()
        {
            { "Slots", () => _filteredSlots },
            { "RunFormations", () => _formations },
            { "ShowingFormations", () => _showingFormations },
            { "ActiveFormations", () => _activeFormations },
            { "SmirkAgainstSlots", () => _smirkAgainstSlots },
            { "AfraidAgainstSlots", () => _afraidAgainstSlots },
        };
        EnvironmentChangedNeuron = new();
        
        _entry = entry ?? Encyclopedia.EntityCategory.DefaultEntry();
        _mingYuan = mingYuan ?? MingYuan.Default;
        _jingJie = jingJie ?? JingJie.LianQi;
        _baseHealth = baseHealth ?? BaseHealthFromJingJie[_jingJie];

        _slots = slots == null ? SlotListModel.Default() : slots.Clone();
        
        _smirkAgainstSlots = smirkAgainstSlots ?? SlotListModel.DefaultWithSize(3);
        _smirkAgainstSlots.Traversal().Do(s => s.Hidden = false);
        _afraidAgainstSlots = afraidAgainstSlots ?? SlotListModel.DefaultWithSize(3);
        _afraidAgainstSlots.Traversal().Do(s => s.Hidden = false);

        if (slotCount == null)
        {
            SetSlotCountFromJingJie(_jingJie);
        }
        else
        {
            SetSlotCount(slotCount.Value);
        }

        Init();
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Slots", () => _filteredSlots },
            { "RunFormations", () => _formations },
            { "ShowingFormations", () => _showingFormations },
            { "ActiveFormations", () => _activeFormations },
            { "SmirkAgainstSlots", () => _smirkAgainstSlots },
            { "AfraidAgainstSlots", () => _afraidAgainstSlots },
        };
        EnvironmentChangedNeuron = new();
        
        _entry = string.IsNullOrEmpty(_entry.GetName()) ? null : Encyclopedia.EntityCategory[_entry.GetName()];
        
        SetSlotCount(_slotCount);
        
        _smirkAgainstSlots ??= SlotListModel.DefaultWithSize(3);
        _smirkAgainstSlots.Traversal().Do(s => s.Hidden = false);
        _afraidAgainstSlots ??= SlotListModel.DefaultWithSize(3);
        _afraidAgainstSlots.Traversal().Do(s => s.Hidden = false);

        Init();
    }

    private void Init()
    {
        _filteredSlots = new FilteredListModel<SkillSlot>(_slots, skillSlot => !skillSlot.Hidden);

        _formations = new();
        _showingFormations = new(_formations, f =>
            f.GetMin() <= f.GetProgress() && _slotCount >= f.GetRequirementFromJingJie(f.GetLowestJingJie()));
        _activeFormations = new(_formations, f => f.IsActivated());
        _slots.Traversal().Do(slot => slot.EnvironmentChangedNeuron.Add(EnvironmentChangedNeuron));
    }
}
