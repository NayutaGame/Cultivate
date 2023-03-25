using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class RunEnemy
{
    private EnemyEntry _entry;
    public EnemyEntry Entry => _entry;

    private CreateEnemyDetails _createEnemyDetails;
    public CreateEnemyDetails CreateEnemyDetails => _createEnemyDetails;

    public int Health;

    private EnemyChipSlot[] _slots;
    public int Start;
    public int Limit;

    public RunEnemy(string name, CreateEnemyDetails d) : this(Encyclopedia.EnemyCategory[name], d) { }
    public RunEnemy(EnemyEntry entry, CreateEnemyDetails d) : this()
    {
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

        SetJingJie(jingJie ?? RunManager.Instance.JingJie);
    }

    public void QuickSetSlotContent(params string[] waiGongNames)
    {
        int diff = _slots.Length - waiGongNames.Length;
        for (int i = waiGongNames.Length - 1; i >= 0; i--)
        {
            if(waiGongNames[i] != null && waiGongNames[i] != "")
                SetSlotContent(diff + i, new RunChip(waiGongNames[i]));
        }
    }

    public void SetJingJie(JingJie jingJie)
    {
        Start = RunManager.WaiGongStartFromJingJie[jingJie];
        Limit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    public EnemyChipSlot GetSlot(int i) => _slots[i];

    public void SetSlotContent(int i, string waiGongName, int[] powers = null)
        => SetSlotContent(i, new RunChip(waiGongName), powers);
    public void SetSlotContent(int i, RunChip waiGong, int[] powers = null)
    {
        _slots[i].Chip = waiGong;
        if(powers != null)
            WuXing.Traversal.Do(wuXing => _slots[i].SetPower(wuXing, powers[wuXing]));
    }

    public string GetEntryDescriptor()
    {
        string dateTime = DateTime.Now.ToString();
        string enemyDescriptor = @"
        new(""敌人{0}"", ""描述"", canCreate: d => true,
            create: (enemy, d) =>
            {{
                enemy.Health = {1};
                enemy.QuickSetSlotContent(""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"", ""{8}"", ""{9}"", ""{10}"", ""{11}"", ""{12}"", ""{13}"");
            }}),";

        return string.Format(enemyDescriptor, dateTime, Health,
            GetSlot(0).GetName() ?? "",
            GetSlot(1).GetName() ?? "",
            GetSlot(2).GetName() ?? "",
            GetSlot(3).GetName() ?? "",
            GetSlot(4).GetName() ?? "",
            GetSlot(5).GetName() ?? "",
            GetSlot(6).GetName() ?? "",
            GetSlot(7).GetName() ?? "",
            GetSlot(8).GetName() ?? "",
            GetSlot(9).GetName() ?? "",
            GetSlot(10).GetName() ?? "",
            GetSlot(11).GetName() ?? ""
        );
    }
}
