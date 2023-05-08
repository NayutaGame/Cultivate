using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using CLLibrary;
using UnityEngine;

public class Arena : Inventory<RunEntity>, GDictionary
{
    private static readonly int ArenaSize = 6;

    private StageReport[] _reports;
    public StageReport[] Reports => _reports;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public StageReport Report;

    public ArenaWaiGongInventory ArenaWaiGongInventory;

    public Arena()
    {
        _accessors = new()
        {
            { "Briefs", () => _reports },
            { "ArenaWaiGongInventory", () => ArenaWaiGongInventory },
        };

        ArenaSize.Do(item => Add(new RunEntity()));
        _reports = new StageReport[ArenaSize * ArenaSize];
        ArenaWaiGongInventory = new();
    }

    // Entity 的 SetEntry 的具体实现
    public void SetEnemy(int index, EntityEntry entityEntry, CreateEntityDetails d)
    {
        this[index] = new RunEntity(entityEntry, d);
    }

    public void Compete()
    {
        for (int y = 0; y < ArenaSize; y++)
        for (int x = 0; x < ArenaSize; x++)
        {
            _reports[y * ArenaSize + x] = StageManager.SimulateBrief(this[y], this[x]);
        }
    }

    public void ShowReport(int i)
    {
        Report = Reports[i];
    }
}
