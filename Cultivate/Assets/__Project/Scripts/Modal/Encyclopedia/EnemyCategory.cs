using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCategory : Category<EnemyEntry>
{
    public EnemyCategory()
    {
        List = new()
        {
            new("鶸", "除了聚气什么都不会的废物",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    return new RunEnemy();
                }),
            new("勥", "一根手指可以把玩家打的爆浆",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(1, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(2, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(3, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(4, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(5, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(6, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(7, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(8, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(9, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(10, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    e.SetSlotContent(11, new RunChip("火剑"), new int[] { 0, 0, 0, 1, 0 });
                    return e;
                }),
        };
    }
}
