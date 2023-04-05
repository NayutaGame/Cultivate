using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCategory : Category<EnemyEntry>
{
    public EnemyCategory()
    {
        List = new()
        {
            new("鶸", "除了聚气什么都不会的废物", canCreate: d => false,
                create: (enemy, d) => { }),

            new("护甲流", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "金11", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "金11", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "金40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "金32", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "金22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "金31", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "金31", 0, 3, new int[]{0, 0, 0, 0, 0});
                }),

            new("暴击流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "金22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "金22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "金20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "金20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "金20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "金20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "金30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "金30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "金30", 0, 3, new int[]{0, 0, 0, 0, 0});
                }),
            new("减甲流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "水20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "水21", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "水40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "水30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "水41", 0, 4, new int[]{0, 0, 0, 0, 0});
                }),

            new("吸血流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "木42", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "木21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "木01", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "木22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "木40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "木01", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "木22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "木40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "木01", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "木22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "木40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木01", 0, 3, new int[]{0, 0, 0, 0, 0});
                }),

            new("格挡流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "木30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "木00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "木00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木31", 0, 3, new int[]{0, 0, 0, 0, 0});
                }),

            new("力量流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "火20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "火22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "火11", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "火11", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "火21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "火30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "火40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "火21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "火30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "火21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "火30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "火40", 0, 4, new int[]{0, 0, 0, 0, 0});
                }),

            new("闪避流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "火41", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "火32", 0, 3, new int[]{0, 0, 0, 0, 0});
                }),

            new("不屈流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "土11", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "土21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "土42", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "", 0, null, new int[]{0, 0, 0, 0, 0});
                }),

            new("高段流", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "土10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "天女散花", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "", 0, null, new int[]{0, 0, 0, 0, 0});
                }),

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

            new("敌人00", "", canCreate: d => d.JingJieRange.Contains(JingJie.LianQi) && !d.AllowBoss,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("火00", "", "金00");
                }),

            new("敌人01", "", canCreate: d => d.JingJieRange.Contains(JingJie.LianQi) && !d.AllowBoss,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("火00", "木00", "火01");
                }),

            new("敌人02", "", canCreate: d => d.JingJieRange.Contains(JingJie.LianQi) && !d.AllowBoss,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("火01", "土00", "金00");
                }),

            new("敌人03", "", canCreate: d => d.JingJieRange.Contains(JingJie.LianQi) && !d.AllowBoss,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("金01", "土01", "");
                }),

            new("敌人04", "", canCreate: d => d.JingJieRange.Contains(JingJie.LianQi) && !d.AllowBoss,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("水00", "火00", "金00");
                }),

            new("练气Boss", "", canCreate: d => d.JingJieRange.Contains(JingJie.LianQi) && d.AllowBoss,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = JingJie.LianQi;
                    enemy.QuickSetSlotContent("木01", "木01", "木01");
                }),

            new("敌人10", "", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("金10", "金01", "水00", "木11", "火01", "土11");
                }),

            new("敌人11", "", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("金11", "水11", "木00", "", "", "");
                }),

            new("敌人12", "", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("火11", "", "土10", "", "", "");
                }),

            new("敌人13", "", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("金11", "水11", "木00", "火11", "", "土10");
                }),

            new("敌人14", "", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("水10", "木11", "火11", "", "土10", "");
                }),

            new("敌人15", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "水11", "金00", "水00", "火00", "木01", "");
                }),

            new("敌人16", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "金10", "金01", "水00", "木11", "火01", "土11");
                }),

            new("敌人17", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = JingJie.ZhuJi;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "金11", "火01", "火11", "金10", "水00", "木11");
                }),

            new("敌人18", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "火10", "火00", "", "木11", "", "水10");
                }),

            new("敌人19", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.ZhuJi),
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.QuickSetSlotContent("", "", "", "", "", "", "", "火00", "水10", "水00", "木01", "金00");
                }),

            new("敌人20", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = 2;
                    enemy.QuickSetSlotContent("", "", "", "", "火10", "金21", "水11", "", "土10", "木11", "水10", "木20");
                }),

            new("敌人21", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = 2;
                    enemy.QuickSetSlotContent("", "", "", "", "木21", "火00", "火00", "土01", "金10", "金10", "木01", "木01");
                }),

            new("敌人22", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = 2;
                    enemy.QuickSetSlotContent("", "", "", "", "水21", "金11", "木01", "火01", "火11", "金10", "水00", "木11");
                }),

            new("敌人23", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = 2;
                    enemy.QuickSetSlotContent("", "", "", "", "火10", "金21", "水11", "木11", "水10", "火01", "", "金20");
                }),

            new("敌人24", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = 2;
                    enemy.QuickSetSlotContent("", "", "", "", "火22", "火11", "土10", "土00", "木11", "", "木22", "土21");
                }),

            new("敌人25", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = 2;
                    enemy.QuickSetSlotContent("", "", "", "", "天衣无缝", "水10", "", "", "火10", "", "", "");
                }),

            new("敌人26", "描述", canCreate: d => d.JingJieRange.Contains(JingJie.JinDan),
                create: (enemy, d) =>
                {
                    enemy.Health = 140;
                    enemy.JingJie = JingJie.JinDan;
                    enemy.QuickSetSlotContent("", "", "", "", "水20", "金21", "金20", "金22", "水10", "金10", "土10", "土21");
                }),

            new("元婴工具人0", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人1", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人2", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人3", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人4", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人5", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人6", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人7", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人8", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("元婴工具人9", "", canCreate: d => d.JingJieRange.Contains(JingJie.YuanYing),
                create: (enemy, d) => { }),
            new("化神工具人0", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人1", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人2", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人3", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人4", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人5", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人6", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人7", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人8", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
            new("化神工具人9", "", canCreate: d => d.JingJieRange.Contains(JingJie.HuaShen),
                create: (enemy, d) => { }),
        };
    }
}
