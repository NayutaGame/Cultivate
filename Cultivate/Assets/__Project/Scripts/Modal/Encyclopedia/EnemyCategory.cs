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

            new("练气00", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = 0;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "金00", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("练气01", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = 0;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "火01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "土00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "土00", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("练气02", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = 0;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("练气03", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = 0;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "土01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "土01", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("练气04", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = 0;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "金01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "金02", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("练气05", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 40;
                    enemy.JingJie = 0;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "木00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基00", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "金11", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "木00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金02", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "水00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基01", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "金11", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "木00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "", 0, null, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基02", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "水01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "木00", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "火01", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "水11", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木01", 0, 0, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基03", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "火01", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "土01", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "土00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金02", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "吐纳", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "金00", 0, 1, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基10", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "金11", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "金01", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "金02", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金01", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "金02", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "金02", 0, 1, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基11", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "水10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "起", 0, 1, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基12", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "木10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "潜龙在渊", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "木00", 0, 0, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "木01", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "木01", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "木01", 0, 1, new int[]{0, 0, 0, 0, 0});
                }),
            new("筑基13", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 80;
                    enemy.JingJie = 1;
                    enemy.SetSlotContent(0, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "", 0, null, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "火10", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "火01", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "火00", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "火00", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "火00", 0, 1, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "火00", 0, 1, new int[]{0, 0, 0, 0, 0});
                }),
            new("化神00", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "软剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "拔刀", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "重剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "重剑", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "收刀", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "金21", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "金刚剑阵", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "高速剑阵", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "无极剑阵", 0, 4, new int[]{0, 0, 0, 0, 0});
                }),
            // new("化神01", "描述", canCreate: d => true,
            //     create: (enemy, d) =>
            //     {
            //         enemy.Health = 340;
            //         enemy.JingJie = 4;
            //         enemy.SetSlotContent(0, "水20", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(1, "水21", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(2, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(3, "水10", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(4, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(5, "竹剑", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(6, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(7, "竹剑", 0, 2, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(8, "兰剑", 0, 3, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(9, "隐如云", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(10, "兰剑", 0, 4, new int[]{0, 0, 0, 0, 0});
            //         enemy.SetSlotContent(11, "梅剑", 0, 4, new int[]{0, 0, 0, 0, 0});
            //     }),

            new("化神02", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "徐如林", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "木30", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "徐如林", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(4, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "木10", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "木24", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "木20", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "飞龙在天", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "木31", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "木22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "亢龙有悔", 0, 4, new int[]{0, 0, 0, 0, 0});
                }),

            new("化神03", "描述", canCreate: d => true,
                create: (enemy, d) =>
                {
                    enemy.Health = 340;
                    enemy.JingJie = 4;
                    enemy.SetSlotContent(0, "夕象", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(1, "涅槃", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(2, "夜凯", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(3, "夕象", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(5, "火22", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(6, "夕象", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(7, "惊如雷", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(8, "火32", 0, 3, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(9, "火10", 0, 2, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(10, "火40", 0, 4, new int[]{0, 0, 0, 0, 0});
                    enemy.SetSlotContent(11, "火40", 0, 4, new int[]{0, 0, 0, 0, 0});
                }),
        };
    }
}
