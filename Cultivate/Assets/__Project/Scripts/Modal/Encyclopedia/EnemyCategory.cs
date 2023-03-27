using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCategory : Category<EnemyEntry>
{
    public EnemyCategory()
    {
        List = new()
        {
            new("鶸", "除了聚气什么都不会的废物", canCreate: d => true,
                create: (enemy, d) => { }),

            // new("敌人模板", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: (enemy, d) =>
            //     {
            //         enemy.SetSlotContent(0, "蓄力", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(1, "挥砍", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(2, "藤甲", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(3, "蓄力", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(4, "挥砍", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(5, "藤甲", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(6, "蓄力", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(7, "挥砍", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(8, "藤甲", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(9, "火00", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(10, "挥砍", new int[] { 0, 0, 0, 0, 0 });
            //         enemy.SetSlotContent(11, "金00", new int[] { 0, 0, 0, 0, 0 });
            //     }),

            new("敌人00", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("火00", "", "金00");
                }),

            new("敌人01", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("火00", "木00", "火01");
                }),

            new("敌人02", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("水00", "火00", "金00");
                }),

            new("敌人03", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("土00", "金00", "金01");
                }),

            new("敌人04", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("火01", "土00", "金00");
                }),

            new("敌人05", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("木01", "木01", "木01");
                }),

            new("敌人06", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("金01", "土01", "土01");
                }),

            new("敌人10", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("金10", "金01", "水00", "木11", "火01", "土11");
                }),

            new("敌人11", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("金11", "水11", "木00", "", "", "");
                }),

            new("敌人12", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("火11", "", "土10", "", "", "");
                }),

            new("敌人13", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("金11", "水11", "木00", "火11", "", "土10");
                }),

            new("敌人14", "", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("水10", "木11", "火11", "", "土10", "");
                }),

            new("敌人15", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "金10", "金01", "水00", "木11", "火01", "土11");
                }),

            new("敌人16", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "金11", "火01", "火11", "金10", "水00", "木11");
                }),

            new("敌人20", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = JingJie.JinDan;
                    enemy.QuickSetSlotContent("", "", "", "", "水20", "金21", "金20", "金22", "水10", "金10", "土10", "土21");
                }),
        };
    }
}
