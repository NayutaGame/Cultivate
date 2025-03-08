
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class RunEntity : Addressable, IEntity, ISerializationCallbackReceiver, RunClosureListener
{
    public static readonly int MaxSlotCount = 12;
    public static readonly int[] SlotCountFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };
    public static readonly int[] HealthFromJingJie = new[] { 40, 100, 180, 280, 400, 400 };
    public static readonly string NORMAL_KEY = "Normal";
    public static readonly string SMIRK_KEY = "Smirk";
    public static readonly string AFRAID_KEY = "Afraid";
    
    [NonSerialized] public Neuron EnvironmentChangedNeuron;
    [NonSerialized] private FilteredListModel<SkillSlot> _filteredSlots;
    
    [SerializeField] private EntityEntry _entry;
    [SerializeField] private MingYuan _mingYuan;
    [SerializeField] private int _health;
    [SerializeField] private int _slotCount;
    [SerializeField] private JingJie _jingJie;
    [SerializeReference] private SlotListModel _slots;
    [SerializeReference] private SlotListModel _smirkAgainstSlots;
    [SerializeReference] private SlotListModel _afraidAgainstSlots;
    [SerializeField] private int _ladder;
    [OptionalField(VersionAdded = 2)] [SerializeField] private Bound _allowedDifficulty;
    [SerializeField] private bool _inPool;

    #region Accessors
    
    public EntityEntry GetEntry() => _entry;
    public void SetEntry(EntityEntry entry) => _entry = entry;
    public MingYuan GetMingYuan() => _mingYuan;
    public int GetHealth() => _health;
    public void SetHealth(int value) => _health = value;
    public void SetDHealth(int value) => _health += value;
    public BoundedInt GetHealthBounded() => new(GetHealth());
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;
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
        SetSlotCount(SlotCountFromJingJie[jingJie]);
    }
    public int GetLadder() => _ladder;
    public void SetLadder(int value) => _ladder = value;
    public Bound GetAllowedDifficulty() => _allowedDifficulty;
    public void SetAllowedDifficulty(Bound bound) => _allowedDifficulty = bound;
    public bool IsInPool() => _inPool;
    public void SetInPool(bool value) => _inPool = value;
    
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

    #endregion

    #region Procedures

    public void PlacementProcedure()
    {
        PlacementDetails d = new(this);

        TraversalCurrentSlots().Do(slot =>
        {
            slot.PlacedSkill = null;
        });

        RunManager.Instance.Environment.SendEvent(RunClosureDict.WIL_PLACEMENT, d);

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

        RunManager.Instance.Environment.SendEvent(RunClosureDict.DID_PLACEMENT, d);
    }

    public void SecondPlacementProcedure()
    {
        RegisterSecondPlacementProcedure();

        SecondPlacementDetails d = new(this);

        RunManager.Instance.Environment.SendEvent(RunClosureDict.WIL_SECOND_PLACEMENT, d);

        RunManager.Instance.Environment.SendEvent(RunClosureDict.DID_SECOND_PLACEMENT, d);

        UnregisterSecondPlacementProcedure();
    }

    public void RegisterSecondPlacementProcedure()
    {
        _formations.Traversal().Do(f =>
        {
            RunManager.Instance.Environment.RegisterList(f.GetEntry().RunClosures, this);
        });
    }

    public void UnregisterSecondPlacementProcedure()
    {
        _formations.Traversal().Do(f =>
        {
            RunManager.Instance.Environment.UnregisterList(f.GetEntry().RunClosures, this);
        });
    }

    #endregion

    #region Formation

    [NonSerialized] private ListModel<RunFormation> _formations;
    public IEnumerable<RunFormation> TraversalFormations => _formations.Traversal();
    [NonSerialized] private FilteredListModel<RunFormation> _showingFormations;
    [NonSerialized] private FilteredListModel<RunFormation> _activeFormations;
    [NonSerialized] private bool _allowFormation;

    public void InitFormations()
    {
        _formations = new();

        for(int i = 0; i < Encyclopedia.FormationCategory.GetCount(); i++)
        {
            _formations.Add(RunFormation.From(Encyclopedia.FormationCategory[i], 0));
        }
    }

    public void FormationProcedure()
    {
        _allowFormation = true;
        
        RunFormationDetails d = new(this);
        List<RunFormation> toEmphasize = new();

        RunManager.Instance.Environment.SendEvent(RunClosureDict.WIL_FORMATION, d);

        for(int i = 0; i < Encyclopedia.FormationCategory.GetCount(); i++)
        {
            Assert.IsTrue(_formations[i].GetEntry().GetFormationGroupEntry() == Encyclopedia.FormationCategory[i]);

            int oldProgress = _formations[i].GetProgress();
            int newProgress = Encyclopedia.FormationCategory[i].GetProgress(this, d);

            if (oldProgress == newProgress)
                continue;

            _formations[i].SetProgress(newProgress);
            
            if (newProgress >= 1)
                toEmphasize.Add(_formations[i]);
        }

        RunManager.Instance.Environment.SendEvent(RunClosureDict.DID_FORMATION, d);

        // set dirty
        _showingFormations.Refresh();
        _activeFormations.Refresh();

        toEmphasize.Do(f => f.Emphasize());
    }

    public void ClearFormationProcedure()
    {
        _allowFormation = false;
        
        for(int i = 0; i < Encyclopedia.FormationCategory.GetCount(); i++)
        {
            Assert.IsTrue(_formations[i].GetEntry().GetFormationGroupEntry() == Encyclopedia.FormationCategory[i]);
            
            _formations[i].SetProgress(0);
        }

        // set dirty
        _showingFormations.Refresh();
        _activeFormations.Refresh();
    }

    #endregion

    #region Core
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEntity(
        EntityEntry entry = null,
        MingYuan mingYuan = null,
        JingJie? jingJie = null,
        int? health = null,
        int? slotCount = null,
        SlotListModel slots = null,
        SlotListModel smirkAgainstSlots = null,
        SlotListModel afraidAgainstSlots = null,
        int? ladder = null,
        Bound? allowedDifficulty = null,
        bool? inPool = null)
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
        _health = health ?? HealthFromJingJie[_jingJie];

        _slots = slots?.Clone() ?? SlotListModel.Default();
        
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

        _ladder = ladder ?? default;
        _allowedDifficulty = allowedDifficulty ?? default;
        _inPool = inPool ?? default;
    }
    
    public static RunEntity Default()
        => new();
    public static RunEntity Trainer()
        => new(jingJie: JingJie.LianQi, health: 1000000);
    public static RunEntity FromJingJieHealth(JingJie jingJie, int health)
        => new(jingJie: jingJie, health: health);
    public static RunEntity FromTemplate(RunEntity template)
        => new(entry: template._entry, mingYuan: template._mingYuan, jingJie: template._jingJie,
            health: template._health, slotCount: template._slotCount, slots: template._slots,
            smirkAgainstSlots: template._smirkAgainstSlots, afraidAgainstSlots: template._afraidAgainstSlots,
            ladder: template._ladder, allowedDifficulty: template._allowedDifficulty, inPool: template._inPool);
    public static RunEntity FromHardCoded(JingJie? jingJie = null,
        int? baseHealth = null, int? slotCount = null, RunSkill[] skills = null)
        => new(jingJie: jingJie, health: baseHealth, slotCount: slotCount, slots: skills != null ? SlotListModel.FromSkills(skills) : null);

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

        InitFormations();

        _showingFormations = new(_formations, f =>
            _allowFormation &&
            f.GetMin() <= f.GetProgress() &&
            _slotCount >= f.GetRequirementFromJingJie(f.GetLowestJingJie()));
        _activeFormations = new(_formations, f => f.IsActivated());
        _slots.Traversal().Do(slot => slot.EnvironmentChangedNeuron.Add(EnvironmentChangedNeuron));
    }

    #endregion
}
