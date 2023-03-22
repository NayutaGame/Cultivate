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

            /*
            new("炼气1", "蓄力流，弱点很多，正常人都能打过，玩家元素0",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("蓄力"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(1, new RunChip("挥砍"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(2, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(3, new RunChip("蓄力"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(4, new RunChip("挥砍"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(5, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(6, new RunChip("蓄力"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(7, new RunChip("挥砍"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(8, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(9, new RunChip("蓄力"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(10, new RunChip("挥砍"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(11, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    return e;
                }),
            new("炼气2", "有点强度，需要防御对手攻击和抓住机会，玩家元素1",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(1, new RunChip("叶刃"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(2, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(3, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(4, new RunChip("叶刃"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(5, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(6, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(7, new RunChip("叶刃"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(8, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(9, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(10, new RunChip("叶刃"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(11, new RunChip("藤甲"), new int[] { 0, 0, 0, 0, 0 });
                    return e;
                }),

            new("炼气3", "快速卡组轮转，有点强,玩家元素2",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("水刃"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(1, new RunChip("地动术"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(2, new RunChip("大地灵气"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(3, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(4, new RunChip("地动术"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(5, new RunChip("大地灵气"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(6, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(7, new RunChip("地动术"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(8, new RunChip("大地灵气"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(9, new RunChip("水盾"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(10, new RunChip("地动术"), new int[] { 0, 0, 0, 0, 0 });
                    e.SetSlotContent(11, new RunChip("大地灵气"), new int[] { 0, 0, 0, 0, 0 });
                    return e;
                }),
            new("炼气boss", "一根手指可以把玩家打的爆浆,玩家元素3",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("水刃"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(1, new RunChip("叶刃"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(2, new RunChip("藤甲"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(3, new RunChip("水刃"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(4, new RunChip("叶刃"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(5, new RunChip("藤甲"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(6, new RunChip("水刃"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(7, new RunChip("叶刃"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(8, new RunChip("藤甲"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(9, new RunChip("水刃"), new int[] { 0, 1, 0, 0, 0 });
                    e.SetSlotContent(10, new RunChip("叶刃"), new int[] { 0, 0, 1, 0, 0 });
                    e.SetSlotContent(11, new RunChip("藤甲"), new int[] { 0, 0, 1, 0, 0 });
                    return e;
                }),

            new("筑基金", "会施放控制，需要玩家合理应对",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("硬直"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(1, new RunChip("蓄力"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(2, new RunChip("强袭"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(3, new RunChip("蓄力"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(4, new RunChip("挥斧"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(5, new RunChip("挥斧"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(6, new RunChip("硬直"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(7, new RunChip("蓄力"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(8, new RunChip("强袭"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(9, new RunChip("蓄力"), new int[] { 2, 0, 0, 0, 0 });
                    e.SetSlotContent(10, new RunChip("挥斧"), new int[]{ 2, 0, 0, 0, 0 });
                    e.SetSlotContent(11, new RunChip("挥斧"), new int[]{ 2, 0, 0, 0, 0 });
                    return e;
                }),

            new("筑基木", "中毒防御流",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("万物有灵"), new int[] { 0, 0, 3, 0, 0 });
                    e.SetSlotContent(1, new RunChip("见血封喉"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(2, new RunChip("生生不息"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(3, new RunChip("万物有灵"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(4, new RunChip("蜜糖砒霜"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(5, new RunChip("藤甲"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(6, new RunChip("万物有灵"), new int[] { 0, 0, 3, 0, 0 });
                    e.SetSlotContent(7, new RunChip("见血封喉"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(8, new RunChip("生生不息"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(9, new RunChip("万物有灵"), new int[] { 0, 0, 2, 0, 0 });
                    e.SetSlotContent(10, new RunChip("蜜糖砒霜"), new int[]{ 0, 0, 2, 0, 0 });
                    e.SetSlotContent(11, new RunChip("藤甲"), new int[]{ 0, 0, 2, 0, 0 });
                    return e;
                }),

            new("筑基水", "会施放控制，需要玩家合理应对",
                canCreate: d =>
                {
                    return true;
                },
                create: d =>
                {
                    RunEnemy e = new RunEnemy();
                    e.SetSlotContent(0, new RunChip("水之抉择"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(1, new RunChip("水刃"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(2, new RunChip("水盾"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(3, new RunChip("灵气暴涨"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(4, new RunChip("水之守护"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(5, new RunChip("水之抉择"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(6, new RunChip("水之抉择"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(7, new RunChip("水刃"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(8, new RunChip("水盾"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(9, new RunChip("灵气暴涨"), new int[] { 0, 2, 0, 0, 0 });
                    e.SetSlotContent(10, new RunChip("水之守护"), new int[]{ 0, 2, 0, 0, 0 });
                    e.SetSlotContent(11, new RunChip("水之抉择"), new int[]{ 0, 2, 0, 0, 0 });
                    return e;
                }),
                */


            //     new("敌人模板", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("基础加灵力"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            //
            //
            // new("怪兽一", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽一介绍"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("怪兽一攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("怪兽一防御"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽一休息"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            //
            // new("怪兽二", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽二介绍"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("怪兽二防御"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("怪兽二防御"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽二重击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("怪兽二奖励"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            // new("怪兽三", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽三介绍"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("怪兽三攻击强化"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("怪兽三攻击强化"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽三攻击强化"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("怪兽三奖励"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            //
            // new("怪兽四", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽四介绍"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("虚弱"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("怪兽四攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽四重击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("怪兽四攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("怪兽四奖励"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            //
            // new("怪兽五", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽五（精英）"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("怪兽五防御强化"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("易伤"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽五多段攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("怪兽五攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("怪兽四奖励"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            //
            // new("怪兽六", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽六介绍"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("虚弱"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("怪兽六攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽六攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("怪兽六攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("怪兽四奖励"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
            //
            // new("怪兽七", "",
            //     canCreate: d =>
            //     {
            //         return true;
            //     },
            //     create: d =>
            //     {
            //         RunEnemy e = new RunEnemy();
            //         e.SetSlotContent(0, new RunChip("怪兽七介绍"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(1, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(2, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(3, new RunChip("怪兽七攻击"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(4, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(5, new RunChip("自我再生"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(6, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(7, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(8, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(9, new RunChip("聚气术"), new int[] { 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(10, new RunChip("聚气术"), new int[]{ 0, 0, 0, 0, 0 });
            //         e.SetSlotContent(11, new RunChip("怪兽四奖励"), new int[]{ 0, 0, 0, 0, 0 });
            //         return e;
            //     }),
        };
    }
}
