using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFormationEntry
{
    private string _name;

    public string GetName()
        => _name;
    public void SetName(string name)
        => _name = name;

    private JingJie _jingJie;
    public JingJie GetJingJie()
        => _jingJie;

    private string _conditionDescription;
    public string GetConditionDescription()
        => _conditionDescription;

    private string _rewardDescription;
    public string GetRewardDescription()
        => _rewardDescription;

    private Func<RunEntity, FormationArguments, bool> _canActivate;
    public bool CanActivate(RunEntity entity, FormationArguments args)
        => _canActivate(entity, args);

    public SubFormationEntry(JingJie jingJie, string conditionDescription, string rewardDescription,
        Func<RunEntity, FormationArguments, bool> canActivate)
    {
        _jingJie = jingJie;
        _conditionDescription = conditionDescription;
        _rewardDescription = rewardDescription;
        _canActivate = canActivate;
    }
}
