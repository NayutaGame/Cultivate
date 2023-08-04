using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FormationEntry
{
    private string _name;

    public string GetName()
        => _name;
    public void SetName(string name)
        => _name = name;

    private int _order;
    public int GetOrder()
        => _order;
    public void SetOrder(int order)
        => _order = order;

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

    public Func<Formation, StageEntity, Task> _gain;
    public Func<Formation, StageEntity, Task> _lose;

    public Dictionary<string, StageEventCapture> _eventCaptureDict;

    /// <summary>
    /// 定义一个Formation
    /// </summary>
    /// <param name="jingJie">境界</param>
    /// <param name="conditionDescription">条件的描述</param>
    /// <param name="rewardDescription">奖励的描述</param>
    /// <param name="gain">获得时的额外行为</param>
    /// <param name="lose">失去时的额外行为</param>
    /// <param name="eventCaptures">事件捕获</param>
    public FormationEntry(JingJie jingJie, string conditionDescription, string rewardDescription, Func<RunEntity, FormationArguments, bool> canActivate,
        Func<Formation, StageEntity, Task> gain = null,
        Func<Formation, StageEntity, Task> lose = null,
        params StageEventCapture[] eventCaptures
    )
    {
        _jingJie = jingJie;
        _conditionDescription = conditionDescription;
        _rewardDescription = rewardDescription;
        _canActivate = canActivate;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");

        _gain = gain;
        _lose = lose;

        _eventCaptureDict = new Dictionary<string, StageEventCapture>();
        foreach (var stageEventCapture in eventCaptures)
            _eventCaptureDict[stageEventCapture.EventId] = stageEventCapture;
    }
}
