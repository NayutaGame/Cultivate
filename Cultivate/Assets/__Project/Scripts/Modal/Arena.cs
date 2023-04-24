using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using CLLibrary;
using UnityEngine;

public class Arena : Inventory<RunEnemy>, GDictionary
{
    private static readonly int ArenaSize = 6;

    private StageReport[] _reports;
    public StageReport[] Reports => _reports;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public StageReport Report;

    public Arena()
    {
        _accessors = new()
        {
            { "Briefs", () => _reports },
        };

        ArenaSize.Do(item => Add(new RunEnemy()));
        _reports = new StageReport[ArenaSize * ArenaSize];
    }

    public void SetEnemy(int index, int enemyEntryIndex)
    {
        this[index] = new RunEnemy(Encyclopedia.EnemyCategory[enemyEntryIndex], new CreateEnemyDetails(RunManager.Instance.JingJie));
    }

    public void SetRandom(int index)
    {
        int r = RandomManager.Range(0, Encyclopedia.EnemyCategory.GetCount());
        SetEnemy(index, r);
    }

    public void Compete()
    {
        for (int y = 0; y < ArenaSize; y++)
        for (int x = 0; x < ArenaSize; x++)
        {
            StageEntity[] entities = new StageEntity[]
            {
                new StageEnemy(this[y], 0),
                new StageEnemy(this[x], 1),
            };
            _reports[y * ArenaSize + x] = StageManager.SimulateBrief(entities);
        }
    }

    public void ShowReport(int i)
    {
        Report = Reports[i];
    }
}
