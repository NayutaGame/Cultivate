using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RunEntity : GDictionary, IEntityModel
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public static readonly int[] BaseHP = new int[] { 40, 80, 140, 220, 340, 340 };

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
        Start = RunManager.WaiGongStartFromJingJie[_jingJie];
        Limit = RunManager.WaiGongLimitFromJingJie[_jingJie];
        int end = Start + Limit;

        _slots.Length.Do(i =>
        {
            _slots[i].SetReveal(Start <= i && i < end);
        });
    }

    [SerializeReference] private SkillSlot[] _slots;
    public SkillSlot GetSlot(int i)
        => _slots[i];

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
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public RunEntity(EntityEntry entry = null, CreateEntityDetails d = null)
    {
        _accessors = new()
        {
            { "Slots", () => _slots },
        };

        _entry = entry;
        _createEntityDetails = d;

        _slots = new SkillSlot[RunManager.WaiGongLimit];
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = new SkillSlot(this, i);
            _slots[i].EnvironmentChangedEvent += EnvironmentChanged;
        }

        SetJingJie(JingJie.LianQi);
        SetBaseHealth(BaseHP[JingJie.LianQi]);

        if (_entry != null && _createEntityDetails != null)
            _entry.Create(this, _createEntityDetails);
    }

    public void SetDragDropDelegate(InteractDelegate interactDelegate)
    {
        _slots.Do(slot => slot.SetInteractDelegate(interactDelegate));
    }

    public void SetSlotContent(int i, string waiGongName, JingJie? j = null)
    {
        if (string.IsNullOrEmpty(waiGongName))
            return;

        SkillEntry skill = waiGongName;
        JingJie jingJie = j ?? skill.JingJieRange.Start;
        SetSlotContent(i, new RunSkill(skill, jingJie), j);
    }

    public void SetSlotContent(int i, RunSkill skill, JingJie? j = null)
    {
        _slots[i].Skill = skill;
    }

    public void QuickSetSlotContent(params string[] waiGongNames)
    {
        int diff = _slots.Length - waiGongNames.Length;
        for (int i = waiGongNames.Length - 1; i >= 0; i--)
        {
            if (waiGongNames[i] != null && waiGongNames[i] != "")
            {
                SkillEntry skill = waiGongNames[i];
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

    public void TryConsume()
    {
        _slots.Do(slot => slot.TryConsume());
    }

    private void FromEntity(RunEntity entity)
    {
        SetJingJie(entity.GetJingJie());
        SetBaseHealth(entity.GetBaseHealth());
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].Skill = entity._slots[i].Skill;
    }
}
