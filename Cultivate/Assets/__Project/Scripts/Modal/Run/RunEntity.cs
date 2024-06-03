
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RunEntity : Addressable, EntityModel, ISerializationCallbackReceiver
{
    public Neuron EnvironmentChangedNeuron;

    public static readonly int[] BaseHealthFromJingJie = new int[] { 40, 100, 180, 280, 400, 400 };

    [SerializeField] private MingYuan _mingYuan;
    public MingYuan MingYuan => _mingYuan;

    [SerializeField] private int _baseHealth;
    public int GetBaseHealth() => _baseHealth;
    public void SetBaseHealth(int health) => _baseHealth = health;

    [SerializeField] [OptionalField(VersionAdded = 2)] private int _dHealth;
    public int GetDHealth() => _dHealth;
    public void SetDHealth(int dHealth) => _dHealth = dHealth;

    public int GetFinalHealth() => _baseHealth + _dHealth;

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
            _slots[i].SetLocked(i >= _slotCount);
        });

        _filteredSlots?.Refresh();
        
        EnvironmentChangedNeuron.Invoke();
    }
    
    public void SetSlotCountFromJingJie(JingJie jingJie)
    {
        SetSlotCount(RunManager.SlotCountFromJingJie[jingJie]);
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

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.WILL_PLACEMENT, d);

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

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.DID_PLACEMENT, d);
    }

    public void DepleteProcedure()
    {
        DepleteDetails d = new(this);

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.WILL_DEPLETE, d);

        _slots.Traversal().Do(slot => slot.TryDeplete(d));

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.DID_DEPLETE, d);
    }

    #region Formation

    [NonSerialized] private ListModel<RunFormation> _formations;
    public IEnumerable<RunFormation> TraversalFormations => _formations.Traversal();
    [NonSerialized] private FilteredListModel<RunFormation> _showingFormations;

    public void FormationProcedure()
    {
        RunFormationDetails d = new(this);

        _formations.Clear();

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.WILL_FORMATION, d);

        _formations.AddRange(Encyclopedia.FormationCategory.Traversal
            .Map(e => RunFormation.From(e, e.GetProgress(this, d))));

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.DID_FORMATION, d);

        _showingFormations.Refresh();
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
            baseHealth: template._baseHealth, slotCount: template._slotCount, slots: template._slots);
    public static RunEntity FromHardCoded(JingJie? jingJie = null,
        int? baseHealth = null, int? slotCount = null, RunSkill[] skills = null)
        => new(jingJie: jingJie, baseHealth: baseHealth, slotCount: slotCount, slots: skills != null ? SlotListModel.FromSkills(skills) : null);

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEntity(EntityEntry entry = null, MingYuan mingYuan = null, JingJie? jingJie = null,
        int? baseHealth = null, int? slotCount = null, SlotListModel slots = null)
    {
        _accessors = new()
        {
            { "Slots", () => _filteredSlots },
            { "RunFormations", () => _formations },
            { "ShowingFormations", () => _showingFormations },
        };
        EnvironmentChangedNeuron = new();
        
        _entry = entry ?? Encyclopedia.EntityCategory.DefaultEntry();
        _mingYuan = mingYuan ?? MingYuan.Default;
        _jingJie = jingJie ?? JingJie.LianQi;
        _baseHealth = baseHealth ?? BaseHealthFromJingJie[_jingJie];

        _slots = slots == null ? SlotListModel.Default() : slots.Clone();

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
        };
        EnvironmentChangedNeuron = new();
        
        _entry = string.IsNullOrEmpty(_entry.GetName()) ? null : Encyclopedia.EntityCategory[_entry.GetName()];
        
        SetSlotCount(_slotCount);

        Init();
    }

    private void Init()
    {
        _filteredSlots = new FilteredListModel<SkillSlot>(_slots, skillSlot => skillSlot.State != SkillSlot.SkillSlotState.Locked);

        _formations = new();
        _showingFormations = new(_formations, f =>
            f.GetMin() <= f.GetProgress() && _slotCount >= f.GetRequirementFromJingJie(f.GetLowestJingJie()));
        _slots.Traversal().Do(slot => slot.EnvironmentChangedNeuron.Add(EnvironmentChangedNeuron));
    }
}
