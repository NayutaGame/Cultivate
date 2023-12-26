
using System;
using System.Collections.Generic;

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

    private string _trivia;
    public string GetTrivia() => _trivia;

    private Func<RunEntity, FormationArguments, bool> _canActivate;
    public bool CanActivate(RunEntity entity, FormationArguments args)
        => _canActivate(entity, args);

    public Dictionary<int, StageEventDescriptor> _eventDescriptorDict;

    /// <summary>
    /// 定义一个Formation
    /// </summary>
    /// <param name="jingJie">境界</param>
    /// <param name="conditionDescription">条件的描述</param>
    /// <param name="rewardDescription">奖励的描述</param>
    /// <param name="eventDescriptors">事件捕获</param>
    public FormationEntry(JingJie jingJie, string conditionDescription, string rewardDescription, string trivia, Func<RunEntity, FormationArguments, bool> canActivate,
        params StageEventDescriptor[] eventDescriptors
    )
    {
        _jingJie = jingJie;
        _conditionDescription = conditionDescription;
        _rewardDescription = rewardDescription;
        _trivia = trivia;
        _canActivate = canActivate;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");

        _eventDescriptorDict = new Dictionary<int, StageEventDescriptor>();
        foreach (var eventDescriptor in eventDescriptors)
            _eventDescriptorDict[eventDescriptor.EventId] = eventDescriptor;
    }
}
