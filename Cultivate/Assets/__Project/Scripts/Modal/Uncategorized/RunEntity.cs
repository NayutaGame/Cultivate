
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RunEntity : GDictionary, IEntityModel
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged()
    {
        RefreshFormations();
        EnvironmentChangedEvent?.Invoke();
    }

    public static readonly int[] BaseHP = new int[] { 40, 80, 140, 220, 340, 340 };
    // public static readonly int[] BaseHP = new int[] { 40, 50, 70, 100, 140, 140 };

    [SerializeField] private MingYuan _mingYuan;
    public MingYuan MingYuan => _mingYuan;

    [SerializeField] private int _health;
    public int GetBaseHealth() => _health;
    public void SetBaseHealth(int health) => _health = health;

    [OptionalField(VersionAdded = 2)] private int _dHealth;
    public int GetDHealth() => _dHealth;
    public void SetDHealth(int dHealth) => _dHealth = dHealth;

    public int GetFinalHealth() => _health + _dHealth;

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

        _slots.Length.Do(i =>
        {
            _slots[i].SetLocked(!(Start <= i && i < end));
        });
    }

    [SerializeReference] private SkillSlot[] _slots;
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

    #region Formation

    private List<FormationEntry> _activatedFormations;
    public IEnumerable<FormationEntry> TraversalActivatedFormations => _activatedFormations.Traversal();

    private void RefreshFormations()
    {
        _activatedFormations.Clear();
        FormationArguments args = new FormationArguments(this);
        _activatedFormations.AddRange(Encyclopedia.FormationCategory.Traversal
            .Map(f => f.FirstActivatedFormation(this, args))
            .FilterObj(f => f != null));
    }

    #endregion

    private EntityEntry _entry;
    public EntityEntry GetEntry()
        => _entry;
    public void SetEntry(EntityEntry entry)
    {
        _entry = entry;
        FromEntity(new RunEntity(_entry, new CreateEntityDetails(GetJingJie())));
    }

    private CreateEntityDetails _createEntityDetails;
    public CreateEntityDetails CreateEntityDetails => _createEntityDetails;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public RunEntity(EntityEntry entry = null, CreateEntityDetails d = null)
    {
        _accessors = new()
        {
            { "Slots", () => _slots },
            { "ActivatedSubFormations", () => _activatedFormations },
        };

        _entry = entry;
        _createEntityDetails = d;

        _mingYuan = MingYuan.Default;

        _activatedFormations = new List<FormationEntry>();

        _slots = new SkillSlot[RunManager.SkillLimit];
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = new SkillSlot(this, i);
            _slots[i].EnvironmentChangedEvent += EnvironmentChanged;
        }

        SetJingJie(JingJie.LianQi);
        SetBaseHealth(BaseHP[JingJie.LianQi]);

        if (_entry != null && _createEntityDetails != null)
        {
            _entry.Create(this, _createEntityDetails);
            UpdateReveal();
        }
    }

    public void SetSlotContent(int i, string skillName, JingJie? j = null)
    {
        if (string.IsNullOrEmpty(skillName))
            return;

        SkillEntry skill = skillName;
        JingJie jingJie = j ?? skill.JingJieRange.Start;
        SetSlotContent(i, new RunSkill(skill, jingJie), j);
    }

    public void SetSlotContent(int i, RunSkill skill, JingJie? j = null)
    {
        _slots[i].Skill = skill;
    }

    public void QuickSetSlotContent(params string[] skillNames)
    {
        int diff = _slots.Length - skillNames.Length;
        for (int i = skillNames.Length - 1; i >= 0; i--)
        {
            if (skillNames[i] != null && skillNames[i] != "")
            {
                SkillEntry skill = skillNames[i];
                SetSlotContent(diff + i, new RunSkill(skill, skill.JingJieRange.Start));
            }
        }
    }

    public void FromJson(string json)
    {
        try
        {
            FromEntity(JsonUtility.FromJson<RunEntity>(json.Replace('\'', '\"')));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this).Replace('\"', '\'');
    }

    public void TryExhaust()
    {
        _slots.Do(slot => slot.TryExhaust());
    }

    private void FromEntity(RunEntity entity)
    {
        SetJingJie(entity.GetJingJie());
        SetBaseHealth(entity.GetBaseHealth());
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].Skill = entity._slots[i].Skill;
    }
}
