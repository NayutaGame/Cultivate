
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RunEntity : Addressable, IEntityModel, ISerializationCallbackReceiver
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public static readonly int[] BaseHealthFromJingJie = new int[] { 40, 80, 140, 220, 340, 340 };
    // public static readonly int[] BaseHealthFromJingJie = new int[] { 40, 50, 70, 100, 140, 140 };

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

    [SerializeField] [OptionalField(VersionAdded = 2)] private bool _isNormal;
    public bool IsNormal() => _isNormal;
    public void SetNormal(bool value) => _isNormal = value;

    [SerializeField] [OptionalField(VersionAdded = 2)] private bool _isElite;
    public bool IsElite() => _isElite;
    public void SetElite(bool value) => _isElite = value;

    [SerializeField] [OptionalField(VersionAdded = 2)] private bool _isBoss;
    public bool IsBoss() => _isBoss;
    public void SetBoss(bool value) => _isBoss = value;

    #endregion

    [SerializeField] private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie)
    {
        _jingJie = jingJie;
        UpdateReveal();
        EnvironmentChanged();
    }

    [NonSerialized] public int Start;
    [NonSerialized] public int Limit;
    private void UpdateReveal()
    {
        Start = RunManager.SkillStartFromJingJie[_jingJie];
        Limit = RunManager.SkillLimitFromJingJie[_jingJie];
        int end = Start + Limit;

        _slots.Count().Do(i =>
        {
            _slots[i].SetLocked(!(Start <= i && i < end));
        });

        _filteredSlots?.Refresh();
    }

    [SerializeReference] private SlotListModel _slots;
    private FilteredListModel<SkillSlot> _filteredSlots;

    public SkillSlot GetSlot(int i)
        => _slots[i];

    public IEnumerable<SkillSlot> TraversalCurrentSlots()
    {
        for (int i = Start; i < Start + Limit; i++)
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

    public void PlacementProcedure(PlacementDetails d)
    {
        // TraversalCurrentSlots().Do(slot =>
        // {
        //     slot.PlacedSkill = null;
        // });

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.WILL_PLACEMENT, d);

        // TraversalCurrentSlots().Do(slot =>
        // {
        //     slot.PlacedSkill ??= PlacedSkill.FromEntry("聚气术");
        // });

        RunManager.Instance.Environment.EventDict.SendEvent(RunEventDict.DID_PLACEMENT, d);
    }

    #region Formation

    private ListModel<FormationEntry> _activatedFormations;
    public IEnumerable<FormationEntry> TraversalActivatedFormations => _activatedFormations.Traversal();

    public void RefreshFormations()
    {
        _activatedFormations.Clear();
        FormationArguments args = new FormationArguments(this);
        _activatedFormations.AddRange(Encyclopedia.FormationCategory.Traversal
            .Map(f => f.FirstActivatedFormation(this, args))
            .FilterObj(f => f != null));
    }

    #endregion

    [SerializeField] private EntityEntry _entry;
    public EntityEntry GetEntry() => _entry;
    public void SetEntry(EntityEntry entry) => _entry = entry;

    public static RunEntity Default()
        => new();

    public static RunEntity FromJingJieHealth(JingJie jingJie, int health)
    {
        RunEntity e = new();
        e.SetJingJie(jingJie);
        e.SetBaseHealth(health);
        return e;
    }

    public static RunEntity FromTemplate(RunEntity template)
        => new(template);

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEntity(RunEntity template = null)
    {
        _accessors = new()
        {
            { "Slots", () => _filteredSlots },
            { "ActivatedSubFormations", () => _activatedFormations },
        };

        if (template != null)
        {
            _entry = template._entry;
            _mingYuan = template._mingYuan;
            _jingJie = template._jingJie;
            _baseHealth = template._baseHealth;
            _slots = new SlotListModel();

            for (int i = 0; i < RunManager.SkillLimit; i++)
            {
                SkillSlot slot = new SkillSlot(i);
                slot.Skill = template._slots[i].Skill;
                _slots.Add(slot);
            }
        }
        else
        {
            _entry = Encyclopedia.EntityCategory.DefaultEntry();
            _mingYuan = MingYuan.Default;
            _jingJie = JingJie.LianQi;
            _baseHealth = BaseHealthFromJingJie[JingJie.LianQi];
            _slots = new SlotListModel();

            for (int i = 0; i < RunManager.SkillLimit; i++)
            {
                SkillSlot slot = new SkillSlot(i);
                _slots.Add(slot);
            }
        }

        _filteredSlots = new FilteredListModel<SkillSlot>(_slots, skillSlot => skillSlot.State != SkillSlot.SkillSlotState.Locked);

        _activatedFormations = new ListModel<FormationEntry>();
        _slots.Traversal().Do(slot => slot.EnvironmentChangedEvent += EnvironmentChanged);

        UpdateReveal();
        EnvironmentChanged();
    }

    public void TryExhaust()
    {
        _slots.Traversal().Do(slot => slot.TryExhaust());
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Slots", () => _slots },
            { "ActivatedSubFormations", () => _activatedFormations },
        };
        _entry = string.IsNullOrEmpty(_entry.Name) ? null : Encyclopedia.EntityCategory[_entry.Name];

        _activatedFormations = new ListModel<FormationEntry>();
        _slots.Traversal().Do(slot => slot.EnvironmentChangedEvent += EnvironmentChanged);

        UpdateReveal();
        EnvironmentChanged();
    }

    public SkillSlot FindSlotWithSkillEntry(SkillEntry entry)
    {
        return TraversalCurrentSlots().FirstObj(slot => slot.Skill?.GetEntry() == entry);
    }
}
