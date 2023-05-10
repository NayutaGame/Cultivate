using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCategory : Category<EntityEntry>
{
    public EntityCategory()
    {
        List = new()
        {
            new("鶸", "除了聚气什么都不会的废物", canCreate: d => d.JingJie < JingJie.JinDan,
                create: (entity, d) => { }),

            new("金丹水吸血", "永久吸血", canCreate: d => (d.Step == 0 || d.Step == 1) && d.JingJie == JingJie.JinDan,
                create: (entity, d) =>
                {
                    string json = @"
{'_health':140,'_jingJie':{'_index':2,'_name':'金丹'},'_slots':[{'rid':1000},{'rid':1001},{'rid':1002},{'rid':1003},{'rid':1004},{'rid':1005},{'rid':1006},{'rid':1007},{'rid':1008},{'rid':1009},{'rid':1010},{'rid':1011}],'references':{'version':2,'RefIds':[{'rid':-2,'type':{'class':'','ns':'','asm':''},'data':{}},{'rid':1000,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':0,'_skill':{'rid':-2}}},{'rid':1001,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':1,'_skill':{'rid':-2}}},{'rid':1002,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':2,'_skill':{'rid':-2}}},{'rid':1003,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':3,'_skill':{'rid':-2}}},{'rid':1004,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':4,'_skill':{'rid':1013}}},{'rid':1005,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':5,'_skill':{'rid':1014}}},{'rid':1006,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':6,'_skill':{'rid':1015}}},{'rid':1007,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':7,'_skill':{'rid':1016}}},{'rid':1008,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':8,'_skill':{'rid':1017}}},{'rid':1009,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':9,'_skill':{'rid':1018}}},{'rid':1010,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':10,'_skill':{'rid':1019}}},{'rid':1011,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':11,'_skill':{'rid':1020}}},{'rid':1012,'type':{'class':'RunEntity','ns':'','asm':'Assembly-CSharp'},'data':{'_health':140,'_jingJie':{'_index':2,'_name':'金丹'},'_slots':[{'rid':1000},{'rid':1001},{'rid':1002},{'rid':1003},{'rid':1004},{'rid':1005},{'rid':1006},{'rid':1007},{'rid':1008},{'rid':1009},{'rid':1010},{'rid':1011}]}},{'rid':1013,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'气吞山河'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1014,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'吐纳'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1015,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水22'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1016,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'永久吸血'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1017,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水25'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1018,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水25'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1019,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水25'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1020,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水25'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}}]}}
";
                    entity.FromJson(json);
                    // entity.SetHealth(RunEntity.BaseHP[JingJie.JinDan]);
                    // entity.SetJingJie(JingJie.JinDan);
                    // switch (JingJie.JinDan)
                    // {
                    //     case 0:
                    //         break;
                    //     case 1:
                    //         break;
                    //     case 2:
                    //         entity.SetSlotContent(4, "气吞山河", 2);
                    //         entity.SetSlotContent(5, "吐纳", 2);
                    //         entity.SetSlotContent(6, "水22", 2);
                    //         entity.SetSlotContent(7, "永久吸血", 2);
                    //         entity.SetSlotContent(8, "水25", 2);
                    //         entity.SetSlotContent(9, "水25", 2);
                    //         entity.SetSlotContent(10, "水25", 2);
                    //         entity.SetSlotContent(11, "水25", 2);
                    //         break;
                    //     case 3:
                    //         break;
                    //     case 4:
                    //     case 5:
                    //     default:
                    //         break;
                    // }
                }),

            new("金丹水格挡", "减甲刷格挡", canCreate: d => d.Step == 2 && d.JingJie == JingJie.JinDan,
                create: (entity, d) =>
                {
                    string json = @"
{'_health':140,'_jingJie':{'_index':2,'_name':'金丹'},'_slots':[{'rid':1000},{'rid':1001},{'rid':1002},{'rid':1003},{'rid':1004},{'rid':1005},{'rid':1006},{'rid':1007},{'rid':1008},{'rid':1009},{'rid':1010},{'rid':1011}],'references':{'version':2,'RefIds':[{'rid':-2,'type':{'class':'','ns':'','asm':''},'data':{}},{'rid':1000,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':0,'_skill':{'rid':-2}}},{'rid':1001,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':1,'_skill':{'rid':-2}}},{'rid':1002,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':2,'_skill':{'rid':-2}}},{'rid':1003,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':3,'_skill':{'rid':-2}}},{'rid':1004,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':4,'_skill':{'rid':1013}}},{'rid':1005,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':5,'_skill':{'rid':1014}}},{'rid':1006,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':6,'_skill':{'rid':1015}}},{'rid':1007,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':7,'_skill':{'rid':1016}}},{'rid':1008,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':8,'_skill':{'rid':1017}}},{'rid':1009,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':9,'_skill':{'rid':1018}}},{'rid':1010,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':10,'_skill':{'rid':1019}}},{'rid':1011,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':11,'_skill':{'rid':1020}}},{'rid':1012,'type':{'class':'RunEntity','ns':'','asm':'Assembly-CSharp'},'data':{'_health':140,'_jingJie':{'_index':2,'_name':'金丹'},'_slots':[{'rid':1000},{'rid':1001},{'rid':1002},{'rid':1003},{'rid':1004},{'rid':1005},{'rid':1006},{'rid':1007},{'rid':1008},{'rid':1009},{'rid':1010},{'rid':1011}]}},{'rid':1013,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'刺穴'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1014,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'起'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1015,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'金20'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1016,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水30'},'_jingJie':{'_index':3,'_name':'元婴'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1017,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水25'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1018,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'气吞山河'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1019,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'身骑白马'},'_jingJie':{'_index':1,'_name':'筑基'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1020,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'水30'},'_jingJie':{'_index':3,'_name':'元婴'},'_runUsedTimes':0,'_runEquippedTimes':0}}]}}
";
                    entity.FromJson(json);
                    // entity.SetHealth(RunEntity.BaseHP[JingJie.JinDan]);
                    // entity.SetJingJie(JingJie.JinDan);
                    // switch (JingJie.JinDan)
                    // {
                    //     case 0:
                    //         break;
                    //     case 1:
                    //         break;
                    //     case 2:
                    //         entity.SetSlotContent(4, "刺穴", 2);
                    //         entity.SetSlotContent(5, "起", 2);
                    //         entity.SetSlotContent(6, "金20", 2);
                    //         entity.SetSlotContent(7, "水30", 3);
                    //         entity.SetSlotContent(8, "水25", 2);
                    //         entity.SetSlotContent(9, "气吞山河", 2);
                    //         entity.SetSlotContent(10, "身骑白马", 1);
                    //         entity.SetSlotContent(11, "水30", 3);
                    //         break;
                    //     case 3:
                    //         break;
                    //     case 4:
                    //     case 5:
                    //     default:
                    //         break;
                    // }
                }),

            new("元婴土重剑", "重剑架势", canCreate: d => d.Step == 0 && d.JingJie == JingJie.YuanYing,
                create: (entity, d) =>
                {
                    string json = @"
{'_health':220,'_jingJie':{'_index':3,'_name':'元婴'},'_slots':[{'rid':1000},{'rid':1001},{'rid':1002},{'rid':1003},{'rid':1004},{'rid':1005},{'rid':1006},{'rid':1007},{'rid':1008},{'rid':1009},{'rid':1010},{'rid':1011}],'references':{'version':2,'RefIds':[{'rid':-2,'type':{'class':'','ns':'','asm':''},'data':{}},{'rid':1000,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':0,'_skill':{'rid':-2}}},{'rid':1001,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':1,'_skill':{'rid':-2}}},{'rid':1002,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':2,'_skill':{'rid':1013}}},{'rid':1003,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':3,'_skill':{'rid':1014}}},{'rid':1004,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':4,'_skill':{'rid':1015}}},{'rid':1005,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':5,'_skill':{'rid':1016}}},{'rid':1006,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':6,'_skill':{'rid':1017}}},{'rid':1007,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':7,'_skill':{'rid':1018}}},{'rid':1008,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':8,'_skill':{'rid':1019}}},{'rid':1009,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':9,'_skill':{'rid':1020}}},{'rid':1010,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':10,'_skill':{'rid':1021}}},{'rid':1011,'type':{'class':'SkillSlot','ns':'','asm':'Assembly-CSharp'},'data':{'_owner':{'rid':1012},'_index':11,'_skill':{'rid':1022}}},{'rid':1012,'type':{'class':'RunEntity','ns':'','asm':'Assembly-CSharp'},'data':{'_health':220,'_jingJie':{'_index':3,'_name':'元婴'},'_slots':[{'rid':1000},{'rid':1001},{'rid':1002},{'rid':1003},{'rid':1004},{'rid':1005},{'rid':1006},{'rid':1007},{'rid':1008},{'rid':1009},{'rid':1010},{'rid':1011}]}},{'rid':1013,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'磐石剑阵'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1014,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'收刀'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1015,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'高速剑阵'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1016,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'收刀'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1017,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'磐石剑阵'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1018,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'收刀'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1019,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'拔刀'},'_jingJie':{'_index':3,'_name':'元婴'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1020,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'金刚剑阵'},'_jingJie':{'_index':3,'_name':'元婴'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1021,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'拔刀'},'_jingJie':{'_index':3,'_name':'元婴'},'_runUsedTimes':0,'_runEquippedTimes':0}},{'rid':1022,'type':{'class':'RunSkill','ns':'','asm':'Assembly-CSharp'},'data':{'_entry':{'_name':'磐石剑阵'},'_jingJie':{'_index':2,'_name':'金丹'},'_runUsedTimes':0,'_runEquippedTimes':0}}]}}
";
                    entity.FromJson(json);
                    // entity.SetHealth(RunEntity.BaseHP[JingJie.YuanYing]);
                    // entity.SetJingJie(JingJie.YuanYing);
                    // switch (JingJie.YuanYing)
                    // {
                    //     case 0:
                    //         break;
                    //     case 1:
                    //         break;
                    //     case 2:
                    //         break;
                    //     case 3:
                    //         entity.SetSlotContent(2, "磐石剑阵", 2);
                    //         entity.SetSlotContent(3, "收刀", 2);
                    //         entity.SetSlotContent(4, "高速剑阵", 2);
                    //         entity.SetSlotContent(5, "收刀", 2);
                    //         entity.SetSlotContent(6, "磐石剑阵", 2);
                    //         entity.SetSlotContent(7, "收刀", 2);
                    //         entity.SetSlotContent(8, "拔刀", 3);
                    //         entity.SetSlotContent(9, "金刚剑阵", 3);
                    //         entity.SetSlotContent(10, "拔刀", 3);
                    //         entity.SetSlotContent(11, "磐石剑阵", 2);
                    //         break;
                    //     case 4:
                    //     case 5:
                    //     default:
                    //         break;
                    // }
                }),

            new("元婴金减甲", "无常已至", canCreate: d => d.Step == 1 && d.JingJie == JingJie.YuanYing,
                create: (entity, d) =>
                {
                    entity.SetHealth(RunEntity.BaseHP[JingJie.YuanYing]);
                    entity.SetJingJie(JingJie.YuanYing);
                    switch (JingJie.YuanYing)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            entity.SetSlotContent(2, "刺穴", 1);
                            entity.SetSlotContent(3, "无常已至", 2);
                            entity.SetSlotContent(4, "气吞山河", 2);
                            entity.SetSlotContent(5, "兰剑", 3);
                            entity.SetSlotContent(6, "金20", 2);
                            entity.SetSlotContent(7, "兰剑", 3);
                            entity.SetSlotContent(8, "竹剑", 1);
                            entity.SetSlotContent(9, "兰剑", 3);
                            entity.SetSlotContent(10, "梅剑", 4);
                            entity.SetSlotContent(11, "兰剑", 3);
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("元婴木力量", "天女散花", canCreate: d => d.Step == 2 && d.JingJie == JingJie.YuanYing,
                create: (entity, d) =>
                {
                    entity.SetHealth(RunEntity.BaseHP[JingJie.YuanYing]);
                    entity.SetJingJie(JingJie.YuanYing);
                    switch (JingJie.YuanYing)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            entity.SetSlotContent(2, "心斋", 4);
                            entity.SetSlotContent(3, "盛开", 2);
                            entity.SetSlotContent(4, "潜龙在渊", 1);
                            entity.SetSlotContent(5, "见龙在田", 2);
                            entity.SetSlotContent(6, "飞龙在天", 3);
                            entity.SetSlotContent(7, "心斋", 4);
                            entity.SetSlotContent(8, "一虚一实", 2);
                            entity.SetSlotContent(9, "一虚一实", 2);
                            entity.SetSlotContent(10, "一虚一实", 2);
                            entity.SetSlotContent(11, "天女散花", 4);
                            break;
                        case 4:
                        case 5:
                        default:
                            break;
                    }
                }),

            new("化神金锋锐", "甲转锋锐", canCreate: d => d.Step == 0 && d.JingJie == JingJie.HuaShen,
                create: (entity, d) =>
                {
                    entity.SetHealth(RunEntity.BaseHP[JingJie.HuaShen]);
                    entity.SetJingJie(JingJie.HuaShen);
                    switch (JingJie.HuaShen)
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
                            entity.SetSlotContent(0, "森罗万象", 4);
                            entity.SetSlotContent(1, "起", 1);
                            entity.SetSlotContent(2, "起", 1);
                            entity.SetSlotContent(3, "土40", 4);
                            entity.SetSlotContent(4, "转", 3);
                            entity.SetSlotContent(5, "高速剑阵", 2);
                            entity.SetSlotContent(6, "重剑", 3);
                            entity.SetSlotContent(7, "收刀", 2);
                            entity.SetSlotContent(8, "拔刀", 3);
                            entity.SetSlotContent(9, "重剑", 3);
                            entity.SetSlotContent(10, "金30", 3);
                            entity.SetSlotContent(11, "梅剑", 4);
                            break;
                    }
                }),

            new("化神木白马", "身骑白马", canCreate: d => d.Step == 1 && d.JingJie == JingJie.HuaShen,
                create: (entity, d) =>
                {
                    entity.SetHealth(RunEntity.BaseHP[JingJie.HuaShen]);
                    entity.SetJingJie(JingJie.HuaShen);
                    switch (JingJie.HuaShen)
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
                            entity.SetSlotContent(0, "身骑白马", 4);
                            entity.SetSlotContent(1, "一切皆苦", 4);
                            entity.SetSlotContent(2, "双发", 3);
                            entity.SetSlotContent(3, "回响", 4);
                            entity.SetSlotContent(4, "鹤回翔", 4);
                            entity.SetSlotContent(11, "鹤回翔", 4);
                            break;
                    }
                }),

            new("化神火涅槃", "涅槃", canCreate: d => d.Step == 2 && d.JingJie == JingJie.HuaShen,
                create: (entity, d) =>
                {
                    entity.SetHealth(RunEntity.BaseHP[JingJie.HuaShen]);
                    entity.SetJingJie(JingJie.HuaShen);
                    switch (JingJie.HuaShen)
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
                            entity.SetSlotContent(0, "天衣无缝", 4);
                            entity.SetSlotContent(1, "凤凰涅槃", 4);
                            entity.SetSlotContent(2, "净天地", 4);
                            entity.SetSlotContent(3, "一切皆苦", 1);
                            entity.SetSlotContent(4, "盛开", 2);
                            entity.SetSlotContent(5, "潜龙在渊", 1);
                            entity.SetSlotContent(6, "化劲", 4);
                            entity.SetSlotContent(7, "潜龙在渊", 1);
                            entity.SetSlotContent(8, "化劲", 4);
                            entity.SetSlotContent(9, "潜龙在渊", 1);
                            entity.SetSlotContent(10, "化劲", 4);
                            entity.SetSlotContent(11, "燃灯留烬", 3);
                            break;
                    }
                }),

            // new("噬金甲", "减甲普通金系", canCreate: d => d.AllowNormal,
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "金00", 0);
            //                 entity.SetSlotContent(10, "金00", 0);
            //                 entity.SetSlotContent(11, "金01", 0);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "无常以至", 2);
            //                 entity.SetSlotContent(7, "金00", 0);
            //                 entity.SetSlotContent(8, "金01", 0);
            //                 entity.SetSlotContent(9, "金01", 0);
            //                 entity.SetSlotContent(10, "金01", 0);
            //                 entity.SetSlotContent(11, "竹剑", 1);
            //                 break;
            //             case 2:
            //                 entity.SetSlotContent(4, "刺穴", 1);
            //                 entity.SetSlotContent(5, "无常以至", 2);
            //                 entity.SetSlotContent(6, "水01", 0);
            //                 entity.SetSlotContent(7, "水01", 0);
            //                 entity.SetSlotContent(8, "水01", 0);
            //                 entity.SetSlotContent(9, "水01", 0);
            //                 entity.SetSlotContent(10, "水01", 0);
            //                 entity.SetSlotContent(11, "水01", 0);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "水01", 0);
            //                 entity.SetSlotContent(10, "水01", 0);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(7, "起", 1);
            //                 entity.SetSlotContent(8, "金20", 1);
            //                 entity.SetSlotContent(9, "水01", 0);
            //                 entity.SetSlotContent(10, "水01", 0);
            //                 entity.SetSlotContent(11, "水01", 0);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "木01", 0);
            //                 entity.SetSlotContent(10, "木01", 0);
            //                 entity.SetSlotContent(11, "木01", 0);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "火01", 0);
            //                 entity.SetSlotContent(10, "火01", 0);
            //                 entity.SetSlotContent(11, "火01", 0);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "一切皆苦", 1);
            //                 entity.SetSlotContent(7, "火01", 1);
            //                 entity.SetSlotContent(8, "火11", 1);
            //                 entity.SetSlotContent(9, "土10", 1);
            //                 entity.SetSlotContent(10, "土10", 1);
            //                 entity.SetSlotContent(11, "土10", 1);
            //                 break;
            //             case 2:
            //                 entity.SetSlotContent(4, "一切皆苦", 3);
            //                 entity.SetSlotContent(5, "火01", 2);
            //                 entity.SetSlotContent(6, "火01", 2);
            //                 entity.SetSlotContent(7, "火01", 2);
            //                 entity.SetSlotContent(8, "火11", 2);
            //                 entity.SetSlotContent(9, "土10", 1);
            //                 entity.SetSlotContent(10, "土10", 1);
            //                 entity.SetSlotContent(11, "土10", 1);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(11, "土00", 0);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "起", 1);
            //                 entity.SetSlotContent(10, "起", 1);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "金10", 1);
            //                 entity.SetSlotContent(7, "起", 1);
            //                 entity.SetSlotContent(8, "金10", 1);
            //                 entity.SetSlotContent(9, "起", 1);
            //                 entity.SetSlotContent(10, "金10", 1);
            //                 entity.SetSlotContent(11, "起", 1);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(10, "木02", 1);
            //                 entity.SetSlotContent(11, "火10", 2);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(8, "火10", 2);
            //                 entity.SetSlotContent(9, "火10", 2);
            //                 entity.SetSlotContent(10, "木02", 1);
            //                 entity.SetSlotContent(11, "木02", 1);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "火00", 0);
            //                 entity.SetSlotContent(10, "火00", 0);
            //                 entity.SetSlotContent(11, "火00", 0);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "天衣无缝", 2);
            //                 entity.SetSlotContent(7, "巩固", 1);
            //                 entity.SetSlotContent(8, "巩固", 1);
            //                 entity.SetSlotContent(9, "火01", 0);
            //                 entity.SetSlotContent(10, "火01", 0);
            //                 entity.SetSlotContent(11, "火01", 0);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "土01", 0);
            //                 entity.SetSlotContent(10, "土01", 0);
            //                 entity.SetSlotContent(11, "土00", 0);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "少阳", 2);
            //                 entity.SetSlotContent(7, "土11", 1);
            //                 entity.SetSlotContent(8, "巩固", 1);
            //                 entity.SetSlotContent(9, "巩固", 1);
            //                 entity.SetSlotContent(10, "金刚剑阵", 3);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "金10", 1);
            //                 entity.SetSlotContent(10, "金01", 0);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "刺穴", 1);
            //                 entity.SetSlotContent(7, "无常已至", 2);
            //                 entity.SetSlotContent(8, "起", 1);
            //                 entity.SetSlotContent(9, "竹剑", 1);
            //                 entity.SetSlotContent(10, "金20", 1);
            //                 entity.SetSlotContent(11, "水01", 1);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "吐纳", 4);
            //                 entity.SetSlotContent(7, "水22", 2);
            //                 entity.SetSlotContent(8, "水21", 2);
            //                 entity.SetSlotContent(9, "水10", 1);
            //                 entity.SetSlotContent(10, "朝孔雀", 1);
            //                 entity.SetSlotContent(11, "水11", 1);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 entity.SetSlotContent(9, "火11", 1);
            //                 entity.SetSlotContent(11, "火10", 1);
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "木02", 0);
            //                 entity.SetSlotContent(7, "化劲", 2);
            //                 entity.SetSlotContent(8, "木02", 0);
            //                 entity.SetSlotContent(9, "化劲", 2);
            //                 entity.SetSlotContent(10, "巩固", 1);
            //                 entity.SetSlotContent(11, "承", 2);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = RunEntity.BaseHP[d.JingJie];
            //         entity.SetJingJie(d.JingJie);
            //         switch (d.JingJie)
            //         {
            //             case 0:
            //                 break;
            //             case 1:
            //                 entity.SetSlotContent(6, "拔刀", 2);
            //                 entity.SetSlotContent(7, "磐石剑阵", 2);
            //                 entity.SetSlotContent(8, "高速剑阵", 2);
            //                 entity.SetSlotContent(9, "势如火", 3);
            //                 entity.SetSlotContent(10, "一力降十会", 2);
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
            //     create: (entity, d) =>
            //     {
            //         entity.Health = 220;
            //         entity.JingJie = 3;
            //         entity.SetSlotContent(2, "金01", 1);
            //         entity.SetSlotContent(3, "水10", 2);
            //         entity.SetSlotContent(5, "水00", 1);
            //         entity.SetSlotContent(8, "水00", 1);
            //         entity.SetSlotContent(11, "水01", 1);
            //     }),
            // new("3一拳超人", "描述", canCreate: d => false,
            //     create: (entity, d) =>
            //     {
            //         entity.Health = 220;
            //         entity.JingJie = 3;
            //         entity.SetSlotContent(2, "拔刀", 2);
            //         entity.SetSlotContent(3, "磐石剑阵", 2);
            //         entity.SetSlotContent(4, "巩固", 2);
            //         entity.SetSlotContent(5, "势如火", 3);
            //         entity.SetSlotContent(7, "一力降十会", 2);
            //     }),
            // new("3白泽", "描述", canCreate: d => false,
            //     create: (entity, d) =>
            //     {
            //         entity.Health = 220;
            //         entity.JingJie = 3;
            //         entity.SetSlotContent(2, "天衣无缝", 3);
            //         entity.SetSlotContent(3, "木02", 1);
            //     }),
            // new("4铁臂猿**", "描述", canCreate: d => false,
            //     create: (entity, d) =>
            //     {
            //         entity.Health = 340;
            //         entity.JingJie = 4;
            //         entity.SetSlotContent(0, "巩固", 2);
            //         entity.SetSlotContent(1, "软剑", 3);
            //         entity.SetSlotContent(2, "巩固", 2);
            //         entity.SetSlotContent(3, "拔刀", 2);
            //         entity.SetSlotContent(4, "重剑", 3);
            //         entity.SetSlotContent(5, "重剑", 3);
            //         entity.SetSlotContent(6, "收刀", 4);
            //         entity.SetSlotContent(7, "巩固", 2);
            //         entity.SetSlotContent(8, "高速剑阵", 2);
            //         entity.SetSlotContent(9, "金刚剑阵", 3);
            //         entity.SetSlotContent(10, "高速剑阵", 2);
            //         entity.SetSlotContent(11, "无极剑阵", 4);
            //     }),
            //
            // new("4水**", "描述", canCreate: d => false,
            //     create: (entity, d) =>
            //     {
            //         entity.Health = 340;
            //         entity.JingJie = 4;
            //         entity.SetSlotContent(0, "徐如林", 4);
            //         entity.SetSlotContent(1, "水30", 3);
            //         entity.SetSlotContent(2, "飞龙在天", 3);
            //         entity.SetSlotContent(3, "徐如林", 3);
            //         entity.SetSlotContent(4, "飞龙在天", 3);
            //         entity.SetSlotContent(5, "吐纳", 2);
            //         entity.SetSlotContent(6, "水24", 3);
            //         entity.SetSlotContent(7, "水20", 2);
            //         entity.SetSlotContent(8, "飞龙在天", 3);
            //         entity.SetSlotContent(9, "水31", 3);
            //         entity.SetSlotContent(10, "水22", 2);
            //         entity.SetSlotContent(11, "亢龙有悔", 4);
            //     }),
            //
            // new("4木**", "描述", canCreate: d => false,
            //     create: (entity, d) =>
            //     {
            //         entity.Health = 340;
            //         entity.JingJie = 4;
            //         // entity.SetSlotContent(0, "夕象", 3);
            //         // entity.SetSlotContent(1, "涅槃", 4);
            //         // entity.SetSlotContent(2, "夜凯", 4);
            //         // entity.SetSlotContent(3, "夕象", 3);
            //         // entity.SetSlotContent(5, "木22", 2);
            //         // entity.SetSlotContent(6, "夕象", 3);
            //         // entity.SetSlotContent(7, "惊如雷", 4);
            //         // entity.SetSlotContent(8, "木32", 3);
            //         // entity.SetSlotContent(9, "木10", 2);
            //         // entity.SetSlotContent(10, "木40", 4);
            //         // entity.SetSlotContent(11, "木40", 4);
            //     }),
        };
    }
}
