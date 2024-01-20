
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

    public string _conditionDescription;
    public string GetConditionDescription()
        => _conditionDescription;
    public void SetConditionDescription(string conditionDescription)
        => _conditionDescription = conditionDescription;

    private JingJie _jingJie;
    public JingJie GetJingJie()
        => _jingJie;

    private string _rewardDescription;
    public string GetRewardDescription()
        => _rewardDescription;

    private string _trivia;
    public string GetTrivia() => _trivia;

    private int _requirement;
    public int GetRequirement() => _requirement;

    public Dictionary<int, StageEventDescriptor> _eventDescriptorDict;

    /// <summary>
    /// 定义一个Formation
    /// </summary>
    /// <param name="jingJie">境界</param>
    /// <param name="conditionDescription">条件的描述</param>
    /// <param name="rewardDescription">奖励的描述</param>
    /// <param name="eventDescriptors">事件捕获</param>
    public FormationEntry(JingJie jingJie, string rewardDescription, string trivia, int requirement,
        params StageEventDescriptor[] eventDescriptors
    )
    {
        _jingJie = jingJie;
        _rewardDescription = rewardDescription;
        _trivia = trivia;
        _requirement = requirement;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");

        _eventDescriptorDict = new Dictionary<int, StageEventDescriptor>();
        foreach (var eventDescriptor in eventDescriptors)
            _eventDescriptorDict[eventDescriptor.EventId] = eventDescriptor;
    }
}
