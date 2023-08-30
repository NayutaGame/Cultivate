using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkillRewardDescriptor : RewardDescriptor
{
    public AddSkillRewardDescriptor(SkillEntry entry)
    {

    }

    // public override void Claim()
    // {
    //     bool success = RunManager.Instance.SkillPool.TryDrawSkills(out List<RunSkill> skills, _pred, _wuXing, _jingJie, _count);
    //     if (!success)
    //         throw new Exception();
    //     RunManager.Instance.Battle.SkillInventory.AddSkills(skills);
    // }
    //
    // public override string GetDescription() => _description;
    public override void Claim()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
