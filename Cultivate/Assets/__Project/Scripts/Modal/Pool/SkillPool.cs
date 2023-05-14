using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class SkillPool : Pool<SkillEntry>
{
    public SkillPool()
    {
        4.Do(i => Populate(Encyclopedia.SkillCategory.Traversal.FilterObj(e => e != Encyclopedia.SkillCategory[0])));
    }

    public bool TryDrawSkill(out RunSkill skill, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        Shuffle();

        TryPopItem(out SkillEntry item, s =>
        {
            if (pred != null && !pred(s))
                return false;

            if (wuXing != null && wuXing != s.WuXing)
                return false;

            if (jingJie != null && !s.JingJieRange.Contains(jingJie.Value))
                return false;

            return true;
        });

        item ??= Encyclopedia.SkillCategory[0];
        JingJie itemJingJie = jingJie ?? item.JingJieRange.Start;
        skill = new RunSkill(item, itemJingJie);
        return true;
    }

    public bool TryDrawSkills(out List<RunSkill> skills, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null, int count = 1, bool distinct = true)
    {
        Shuffle();

        skills = new List<RunSkill>();
        List<SkillEntry> skillEntries = new List<SkillEntry>();

        for (int i = 0; i < count; i++)
        {
            TryPopItem(out SkillEntry item, s =>
            {
                if (pred != null && !pred(s))
                    return false;

                if (wuXing != null && wuXing != s.WuXing)
                    return false;

                if (jingJie != null && !s.JingJieRange.Contains(jingJie.Value))
                    return false;

                if (distinct && skillEntries.Contains(s))
                    return false;

                return true;
            });

            item ??= Encyclopedia.SkillCategory[0];
            skillEntries.Add(item);

            JingJie itemJingJie = jingJie ?? item.JingJieRange.Start;
            skills.Add(new RunSkill(item, itemJingJie));
        }

        return true;
    }
}
