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
    public Func<Formation, StageEntity, FormationDetails, Task<FormationDetails>> _anyFormationAdd;
    public Func<Formation, StageEntity, FormationDetails, Task<FormationDetails>> _anyFormationAdded;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _buff;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _anyBuff;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _buffed;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _anyBuffed;

    public Dictionary<string, StageEventCapture> _eventCaptureDict;

    /// <summary>
    /// 定义一个Formation
    /// </summary>
    /// <param name="jingJie">境界</param>
    /// <param name="conditionDescription">条件的描述</param>
    /// <param name="rewardDescription">奖励的描述</param>
    /// <param name="gain">获得时的额外行为</param>
    /// <param name="lose">失去时的额外行为</param>
    /// <param name="anyFormationAdd">任何Formation Add时的额外行为，结算之前</param>
    /// <param name="anyFormationAdded">任何Formation Add时的额外行为，结算之后</param>
    /// <param name="buff">受到Buff时的额外行为，结算之前</param>
    /// <param name="anyBuff">任何人受到Buff时的额外行为，结算之前</param>
    /// <param name="buffed">受到Buff时的额外行为，结算之后</param>
    /// <param name="anyBuffed">任何人受到Buff时的额外行为，结算之后</param>
    /// <param name="eventCaptures">事件捕获</param>
    public FormationEntry(JingJie jingJie, string conditionDescription, string rewardDescription, Func<RunEntity, FormationArguments, bool> canActivate,
        Func<Formation, StageEntity, Task> gain = null,
        Func<Formation, StageEntity, Task> lose = null,
        Func<Formation, StageEntity, FormationDetails, Task<FormationDetails>> anyFormationAdd = null,
        Func<Formation, StageEntity, FormationDetails, Task<FormationDetails>> anyFormationAdded = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> buff = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> anyBuff = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> buffed = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> anyBuffed = null,
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

        _anyFormationAdd = anyFormationAdd;
        _anyFormationAdded = anyFormationAdded;

        _buff = buff;
        _anyBuff = anyBuff;
        _buffed = buffed;
        _anyBuffed = anyBuffed;

        _eventCaptureDict = new Dictionary<string, StageEventCapture>();
        foreach (var stageEventCapture in eventCaptures)
            _eventCaptureDict[stageEventCapture.EventId] = stageEventCapture;
    }
}
