using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class RunEntity : GDictionary, IEntityModel
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public static readonly int[] BaseHP = new int[] { 40, 80, 140, 220, 340, 340 };

    private int _health;
    public int GetHealth() => _health;
    public void SetHealth(int health) => _health = health;

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie)
    {
        _jingJie = jingJie;
        UpdateReveal();
    }

    public int Start;
    public int Limit;
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

    private SkillSlot[] _slots;
    public SkillSlot GetSlot(int i)
        => _slots[i];

    private EntityEntry _entry;
    public EntityEntry GetEntry()
        => _entry;
    public void SetEntry(EntityEntry entry)
    {
        RunEntity entity = new RunEntity(entry, new CreateEntityDetails(GetJingJie()));
        FromEntity(entity);
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
        SetHealth(BaseHP[JingJie.LianQi]);

        if (_entry != null && _createEntityDetails != null)
            _entry.Create(this, _createEntityDetails);
    }

    public void SetDragDropDelegate(DragDropDelegate dragDropDelegate)
    {
        _slots.Do(slot => slot.SetDragDropDelegate(dragDropDelegate));
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
        RunEntity entity = JsonUtility.FromJson<RunEntity>(json);
        FromEntity(entity);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public string GetEntryDescriptor()
    {
        string dateTime = DateTime.Now.ToString();

        StringBuilder sb = new StringBuilder();
        sb.Append(@$"
        new(""敌人{dateTime}"", ""描述"", canCreate: d => true,
            create: (enemy, d) =>
            {{
                enemy.Health = {_health};
                enemy.JingJie = {_jingJie._index};
");

        for (int i = 0; i < _slots.Length; i++)
        {
            SkillSlot s = _slots[i];
            string name = s.GetName();
            if(string.IsNullOrEmpty(name))
                continue;
            string jingJie = s.GetJingJieString();
            sb.Append($"enemy.SetSlotContent({i}, \"{name}\", {jingJie});\n");
        }

        sb.Append(@$"}}),");

        return sb.ToString();
    }

    public void TryConsume()
    {
        _slots.Do(slot => slot.TryConsume());
    }

    private void FromEntity(RunEntity entity)
    {
        SetJingJie(entity.GetJingJie());
        SetHealth(entity.GetHealth());
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].Skill = entity._slots[i].Skill;
    }
}
