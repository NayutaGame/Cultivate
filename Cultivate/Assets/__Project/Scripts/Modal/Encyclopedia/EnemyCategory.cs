using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCategory : Category<EnemyEntry>
{
    public EnemyCategory()
    {
        List = new()
        {
            new("鶸", "除了聚气什么都不会的废物", canCreate: d => d.JingJie < JingJie.JinDan,
                create: (enemy, d) => { }),

            new("金丹水吸血", "永久吸血", canCreate: d => (d.Step == 0 || d.Step == 1) && d.JingJie == JingJie.JinDan,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            enemy.SetSlotContent(4, "气吞山河", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "吐纳", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "水22", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "永久吸血", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "水25", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "水25", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "水25", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "水25", 0, 2, new int[]{0, 0, 0, 0, 0});
                            break;
                        case 3:
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("金丹水格挡", "减甲刷格挡", canCreate: d => d.Step == 2 && d.JingJie == JingJie.JinDan,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            enemy.SetSlotContent(4, "刺穴", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "起", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "金20", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "水25", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "气吞山河", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "身骑白马", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
                            break;
                        case 3:
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("元婴土重剑", "重剑架势", canCreate: d => d.Step == 0 && d.JingJie == JingJie.YuanYing,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            enemy.SetSlotContent(2, "磐石剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(3, "收刀", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(4, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "收刀", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "磐石剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "收刀", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "拔刀", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "金刚剑阵", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "拔刀", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "磐石剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("元婴金减甲", "无常已至", canCreate: d => d.Step == 1 && d.JingJie == JingJie.YuanYing,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            enemy.SetSlotContent(2, "刺穴", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(3, "无常已至", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(4, "气吞山河", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "金20", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "竹剑", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "梅剑", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("元婴木力量", "天女散花", canCreate: d => d.Step == 2 && d.JingJie == JingJie.YuanYing,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            enemy.SetSlotContent(2, "心斋", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(3, "盛开", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(4, "潜龙在渊", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "见龙在田", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "心斋", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "一虚一实", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "一虚一实", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "一虚一实", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "天女散花", 0, 4, new int[]{0, 0, 0, 0, 0});
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("化神金锋锐", "甲转锋锐", canCreate: d => d.Step == 0 && d.JingJie == JingJie.HuaShen,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                        case 5:
                        default:
                            enemy.SetSlotContent(0, "森罗万象", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(1, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(2, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(3, "土40", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(4, "转", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "重剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "收刀", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "拔刀", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "重剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "金30", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "梅剑", 0, 4, new int[]{0, 0, 0, 0, 0});
                            break;
                    }
                }),

            new("化神木白马", "身骑白马", canCreate: d => d.Step == 1 && d.JingJie == JingJie.HuaShen,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                        case 5:
                        default:
                            enemy.SetSlotContent(0, "身骑白马", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(1, "一切皆苦", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(2, "双发", 0, 3, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(3, "回响", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(4, "鹤回翔", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "鹤回翔", 0, 4, new int[]{0, 0, 0, 0, 0});
                            break;
                    }
                }),

            new("化神火涅槃", "涅槃", canCreate: d => d.Step == 2 && d.JingJie == JingJie.HuaShen,
                create: (enemy, d) =>
                {
                    enemy.Health = RunHero.BaseHP[d.JingJie];
                    enemy.JingJie = d.JingJie;
                    switch (d.JingJie)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                        case 5:
                        default:
                            enemy.SetSlotContent(0, "天衣无缝", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(1, "凤凰涅槃", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(2, "净天地", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(3, "一切皆苦", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(4, "盛开", 0, 2, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(5, "潜龙在渊", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(6, "化劲", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(7, "潜龙在渊", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(8, "化劲", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(9, "潜龙在渊", 0, 1, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(10, "化劲", 0, 4, new int[]{0, 0, 0, 0, 0});
                            enemy.SetSlotContent(11, "燃灯留烬", 0, 3, new int[]{0, 0, 0, 0, 0});
                            break;
                    }
                }),

            // new("噬金甲", "减甲普通金系", canCreate: d => d.AllowNormal,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "金00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "金00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "无常以至", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "金00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "竹剑", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 enemy.SetSlotContent(4, "刺穴", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(5, "无常以至", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(6, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("墨蛟", "普通水系吸血", canCreate: d => d.AllowNormal,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(7, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "金20", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("渊虾", "普通木系穿透", canCreate: d => d.AllowNormal,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("火蟾", "普通火系灼烧", canCreate: d => d.AllowNormal,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "一切皆苦", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "火01", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "火11", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 enemy.SetSlotContent(4, "一切皆苦", 0, 3, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(5, "火01", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(6, "火01", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "火01", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "火11", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("推山兽", "普通土系高伤", canCreate: d => d.AllowNormal,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(11, "土00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("白泽", "精英金系锋锐", canCreate: d => d.AllowElite,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "金10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "金10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "金10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("鲲", "精英水系格挡", canCreate: d => d.AllowElite,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 break;
            //             case 1:
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("毕方", "精英木系力量多段", canCreate: d => d.AllowElite,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(10, "木02", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "火10", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(8, "火10", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "火10", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "木02", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "木02", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("九尾狐", "精英火系天衣无缝", canCreate: d => d.AllowElite,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "火00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "火00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "火00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "天衣无缝", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "巩固", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "巩固", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("麒麟", "精英土系加甲", canCreate: d => d.AllowElite,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "土01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "土01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "土00", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "少阳", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "土11", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "巩固", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "巩固", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "金刚剑阵", 0, 3, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("白虎", "首领金系减甲", canCreate: d => d.AllowBoss,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "金10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "刺穴", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "无常已至", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "竹剑", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "金20", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "水01", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("玄武", "首领水系格挡", canCreate: d => d.AllowBoss,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "吐纳", 0, 4, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "水22", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "水21", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "朝孔雀", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "水11", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("青龙", "首领木系闪避", canCreate: d => d.AllowBoss,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 break;
            //             case 1:
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("朱雀", "首领火系涅槃灼烧", canCreate: d => d.AllowBoss,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 enemy.SetSlotContent(9, "火11", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "木02", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "化劲", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "木02", 0, 0, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "化劲", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "巩固", 0, 1, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(11, "承", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("黄龙", "首领土系高伤", canCreate: d => d.AllowBoss,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = RunHero.BaseHP[d.JingJie];
            //         enemy.JingJie = d.JingJie;
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 break;
            //             case 1:
            //                 enemy.SetSlotContent(6, "拔刀", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(7, "磐石剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(8, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(9, "势如火", 0, 3, new int[]{0, 0, 0, 0, 0});
            //                 enemy.SetSlotContent(10, "一力降十会", 0, 2, new int[]{0, 0, 0, 0, 0});
            //                 break;
            //             case 2:
            //                 break;
            //             case 3:
            //                 break;
            //             case 4:
            //             case 5:
            //             default:
            //                 break;
            //         }
            //     }),
            //
            // new("3旋龟", "描述", canCreate: d => false,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 220;
            //         enemy.JingJie = 3;
            //         enemy.SetSlotContent(2, "金01", 0, 1, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(3, "水10", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(5, "水00", 0, 1, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(8, "水00", 0, 1, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(11, "水01", 0, 1, new int[]{0, 0, 0, 0, 0});
            //     }),
            // new("3一拳超人", "描述", canCreate: d => false,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 220;
            //         enemy.JingJie = 3;
            //         enemy.SetSlotContent(2, "拔刀", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(3, "磐石剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(4, "巩固", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(5, "势如火", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(7, "一力降十会", 0, 2, new int[]{0, 0, 0, 0, 0});
            //     }),
            // new("3白泽", "描述", canCreate: d => false,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 220;
            //         enemy.JingJie = 3;
            //         enemy.SetSlotContent(2, "天衣无缝", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(3, "木02", 0, 1, new int[]{0, 0, 0, 0, 0});
            //     }),
            // new("4铁臂猿**", "描述", canCreate: d => false,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 340;
            //         enemy.JingJie = 4;
            //         enemy.SetSlotContent(0, "巩固", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(1, "软剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(2, "巩固", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(3, "拔刀", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(4, "重剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(5, "重剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(6, "收刀", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(7, "巩固", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(8, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(9, "金刚剑阵", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(10, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(11, "无极剑阵", 0, 4, new int[]{0, 0, 0, 0, 0});
            //     }),
            //
            // new("4水**", "描述", canCreate: d => false,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 340;
            //         enemy.JingJie = 4;
            //         enemy.SetSlotContent(0, "徐如林", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(1, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(2, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(3, "徐如林", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(4, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(5, "吐纳", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(6, "水24", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(7, "水20", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(8, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(9, "水31", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(10, "水22", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(11, "亢龙有悔", 0, 4, new int[]{0, 0, 0, 0, 0});
            //     }),
            //
            // new("4木**", "描述", canCreate: d => false,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 340;
            //         enemy.JingJie = 4;
            //         // enemy.SetSlotContent(0, "夕象", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(1, "涅槃", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(2, "夜凯", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(3, "夕象", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(5, "木22", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(6, "夕象", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(7, "惊如雷", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(8, "木32", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(9, "木10", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(10, "木40", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         // enemy.SetSlotContent(11, "木40", 0, 4, new int[]{0, 0, 0, 0, 0});
            //     }),
        };
    }
}
