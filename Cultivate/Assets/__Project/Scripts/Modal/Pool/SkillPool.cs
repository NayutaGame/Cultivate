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
        4.Do(i => Populate(Encyclopedia.SkillCategory.Traversal.FilterObj(e => e.Name != "聚气术")));
    }

    public bool TryDrawSkill(out RunSkill skill, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        Shuffle();

        TryPopItem(s =>
        {
            if (pred != null && !pred(s))
                return false;

            if (wuXing != null && wuXing != s.WuXing)
                return false;

            if (jingJie != null && !s.JingJieRange.Contains(jingJie.Value))
                return false;

            return true;
        }, out SkillEntry item);

        if (item == null)
        {
            skill = null;
            return false;
        }

        JingJie itemJingJie = jingJie ?? item.JingJieRange.Start;
        skill = new RunSkill(item, itemJingJie);
        return true;
    }

    public bool TryDrawSkills(out List<RunSkill> skills, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null, int count = 1)
    {
        Shuffle();

        skills = new List<RunSkill>();
        for (int i = 0; i < count; i++)
        {
            TryPopItem(s =>
            {
                if (pred != null && !pred(s))
                    return false;

                if (wuXing != null && wuXing != s.WuXing)
                    return false;

                if (jingJie != null && !s.JingJieRange.Contains(jingJie.Value))
                    return false;

                return true;
            }, out SkillEntry item);

            if (item == null)
                return false;

            JingJie itemJingJie = jingJie ?? item.JingJieRange.Start;
            skills.Add(new RunSkill(item, itemJingJie));
        }

        return true;
    }
}
