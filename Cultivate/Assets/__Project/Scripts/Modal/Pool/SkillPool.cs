using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class SkillPool : Pool<SkillEntry>
{
    public bool TryDrawSkill(out RunSkill skill, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
        => TryDrawSkill(out skill, new(pred, wuXing, jingJie));
    public bool TryDrawSkill(out RunSkill skill, DrawSkillDetails d)
    {
        Shuffle();

        TryPopItem(out SkillEntry item, d.CanDraw);

        item ??= Encyclopedia.SkillCategory[0];

        JingJie jingJie = d._jingJie ?? JingJie.LianQi;
        jingJie = Mathf.Clamp(jingJie, item.LowestJingJie, item.HighestJingJie);

        skill = RunSkill.FromEntry(item, jingJie);
        return true;
    }

    public bool TryDrawSkills(out List<RunSkill> skills, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null, int count = 1, bool distinct = true, bool consume = true)
        => TryDrawSkills(out skills, new DrawSkillsDetails(pred, wuXing, jingJie, count, distinct, consume));
    public bool TryDrawSkills(out List<RunSkill> skills, DrawSkillsDetails d)
    {
        Shuffle();

        skills = new List<RunSkill>();
        List<SkillEntry> skillEntries = new List<SkillEntry>();

        for (int i = 0; i < d._count; i++)
        {
            TryPopItem(out SkillEntry item, s =>
            {
                if (!d.CanDraw(s))
                    return false;

                if (d._distinct && skillEntries.Contains(s))
                    return false;

                return true;
            });

            item ??= Encyclopedia.SkillCategory[0];
            skillEntries.Add(item);

            JingJie jingJie = d._jingJie ?? JingJie.LianQi;
            jingJie = Mathf.Clamp(jingJie, item.LowestJingJie, item.HighestJingJie);

            skills.Add(RunSkill.FromEntry(item, jingJie));
        }

        if (!d._consume)
        {
            Populate(skills.FilterObj(s => s.GetEntry() != Encyclopedia.SkillCategory[0]).Map(s => s.GetEntry()));
        }

        return true;
    }
}
