using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class RunEnemy : GDictionary
{
    private EnemyEntry _entry;
    public EnemyEntry Entry => _entry;

    private CreateEnemyDetails _createEnemyDetails;
    public CreateEnemyDetails CreateEnemyDetails => _createEnemyDetails;

    public int Health;
    private EnemyChipSlot[] _slots;

    private JingJie _jingJie;
    public JingJie JingJie
    {
        get => _jingJie;
        set
        {
            _jingJie = value;

            Start = RunManager.WaiGongStartFromJingJie[_jingJie];
            Limit = RunManager.WaiGongLimitFromJingJie[_jingJie];

            int end = Start + Limit;
            _slots.Length.Do(i =>
            {
                _slots[i].IsReveal = Start <= i && i < end;
            });
        }
    }

    public int Start;
    public int Limit;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public RunEnemy(string name, CreateEnemyDetails d) : this(Encyclopedia.EnemyCategory[name], d) { }
    public RunEnemy(EnemyEntry entry, CreateEnemyDetails d) : this()
    {
        _accessors = new()
        {
            { "Slots", () => _slots },
        };

        _entry = entry;
        _createEnemyDetails = d;

        _entry.Create(this, _createEnemyDetails);
    }

    public RunEnemy(int health = 40, JingJie? jingJie = null)
    {
        Health = health;
        _slots = new EnemyChipSlot[RunManager.WaiGongLimit];
        for (int i = 0; i < _slots.Length; i++)
            _slots[i] = new EnemyChipSlot(i);

        JingJie = jingJie ?? RunManager.Instance.JingJie;
    }

    public void QuickSetSlotContent(params string[] waiGongNames)
    {
        int diff = _slots.Length - waiGongNames.Length;
        for (int i = waiGongNames.Length - 1; i >= 0; i--)
        {
            if (waiGongNames[i] != null && waiGongNames[i] != "")
            {
                ChipEntry chip = waiGongNames[i];
                SetSlotContent(diff + i, new RunChip(chip, chip.JingJieRange.Start));
            }
        }
    }

    public EnemyChipSlot GetSlot(int i) => _slots[i];

    public void SetSlotContent(int i, string waiGongName, int l = 0, JingJie? j = null, int[] p = null)
    {
        if (string.IsNullOrEmpty(waiGongName))
            return;

        ChipEntry chip = waiGongName;
        JingJie jingJie = j ?? chip.JingJieRange.Start;
        SetSlotContent(i, new RunChip(chip, jingJie, l), l, j, p);
    }

    public void SetSlotContent(int i, RunChip waiGong, int l = 0, JingJie? j = null, int[] p = null)
    {
        _slots[i].Chip = waiGong;
        if(p != null)
            WuXing.Traversal.Do(wuXing => _slots[i].SetPower(wuXing, p[wuXing]));
    }

    public string GetEntryDescriptor()
    {
        string dateTime = DateTime.Now.ToString();

        StringBuilder sb = new StringBuilder();
        sb.Append(@$"
        new(""敌人{dateTime}"", ""描述"", canCreate: d => true,
            create: (enemy, d) =>
            {{
                enemy.Health = {Health};
                enemy.JingJie = {_jingJie._index};
");

        for (int i = 0; i < _slots.Length; i++)
        {
            EnemyChipSlot s = GetSlot(i);
            string name = s.GetName();
            if(string.IsNullOrEmpty(name))
                continue;
            int level = s.Chip?.Level ?? 0;
            string jingJie = s.GetJingJie()?._index.ToString() ?? "null";
            int jin = s.GetPower(WuXing.Jin);
            int shui = s.GetPower(WuXing.Shui);
            int mu = s.GetPower(WuXing.Mu);
            int huo = s.GetPower(WuXing.Huo);
            int tu = s.GetPower(WuXing.Tu);
            sb.Append($"enemy.SetSlotContent({i}, \"{name}\", {level}, {jingJie}, new int[]{{{jin}, {shui}, {mu}, {huo}, {tu}}});\n");
        }

        sb.Append(@$"}}),");

        return sb.ToString();
    }
}
