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
    public Func<Formation, StageEntity, Task> _startStage;
    public Func<Formation, StageEntity, Task> _endStage;
    public Func<Formation, TurnDetails, Task> _startTurn;
    public Func<Formation, TurnDetails, Task> _endTurn;
    public Func<Formation, StageEntity, Task> _startRound;
    public Func<Formation, StageEntity, Task> _endRound;
    public Func<Formation, StepDetails, Task> _startStep;
    public Func<Formation, StepDetails, Task> _endStep;
    public Func<Formation, AttackDetails, Task> _attack;
    public Func<Formation, AttackDetails, Task> _attacked;
    public Func<Formation, DamageDetails, Task> _damage;
    public Func<Formation, DamageDetails, Task> _damaged;
    public Func<Formation, Task> _killed;
    public Func<Formation, Task> _kill;
    public Func<Formation, HealDetails, Task> _heal;
    public Func<Formation, HealDetails, Task> _healed;
    public Func<Formation, ArmorGainDetails, Task> _armorGain;
    public Func<Formation, ArmorGainDetails, Task> _armorGained;
    public Func<Formation, ArmorLoseDetails, Task> _armorLose;
    public Func<Formation, ArmorLoseDetails, Task> _armorLost;
    public Func<Formation, EvadeDetails, Task> _evaded;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _buff;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _anyBuff;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _buffed;
    public Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> _anyBuffed;
    public Func<Formation, ExhaustDetails, Task> _consumed;

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
    /// <param name="startStage">Stage开始时的额外行为</param>
    /// <param name="endStage">Stage结束时的额外行为</param>
    /// <param name="startTurn">Turn开始时的额外行为</param>
    /// <param name="endTurn">Turn结束时的额外行为</param>
    /// <param name="startRound">Round开始时的额外行为</param>
    /// <param name="endRound">Round结束时的额外行为</param>
    /// <param name="startStep">Step开始是的额外行为</param>
    /// <param name="endStep">Step结束时的额外行为</param>
    /// <param name="attack">攻击时的额外行为</param>
    /// <param name="attacked">受攻击时的额外行为</param>
    /// <param name="damage">伤害时的额外行为</param>
    /// <param name="damaged">受伤害时的额外行为</param>
    /// <param name="killed">被击杀时的额外行为</param>
    /// <param name="kill">击杀时的额外行为</param>
    /// <param name="heal">治疗时的额外行为</param>
    /// <param name="healed">被治疗时的额外行为</param>
    /// <param name="armorGain">给予护甲时的额外行为</param>
    /// <param name="armorGained">接受护甲时的额外行为</param>
    /// <param name="armorLose">使失去护甲时的额外行为</param>
    /// <param name="armorLost">失去护甲时的额外行为</param>
    /// <param name="evaded">闪避时的额外行为</param>
    /// <param name="buff">受到Buff时的额外行为，结算之前</param>
    /// <param name="anyBuff">任何人受到Buff时的额外行为，结算之前</param>
    /// <param name="buffed">受到Buff时的额外行为，结算之后</param>
    /// <param name="anyBuffed">任何人受到Buff时的额外行为，结算之后</param>
    /// <param name="consumed">被消耗时的额外行动</param>
    public FormationEntry(JingJie jingJie, string conditionDescription, string rewardDescription, Func<RunEntity, FormationArguments, bool> canActivate,
        Func<Formation, StageEntity, Task> gain = null,
        Func<Formation, StageEntity, Task> lose = null,
        Func<Formation, StageEntity, FormationDetails, Task<FormationDetails>> anyFormationAdd = null,
        Func<Formation, StageEntity, FormationDetails, Task<FormationDetails>> anyFormationAdded = null,
        Func<Formation, StageEntity, Task> startStage = null,
        Func<Formation, StageEntity, Task> endStage = null,
        Func<Formation, TurnDetails, Task> startTurn = null,
        Func<Formation, TurnDetails, Task> endTurn = null,
        Func<Formation, StageEntity, Task> startRound = null,
        Func<Formation, StageEntity, Task> endRound = null,
        Func<Formation, StepDetails, Task> startStep = null,
        Func<Formation, StepDetails, Task> endStep = null,
        Func<Formation, AttackDetails, Task> attack = null,
        Func<Formation, AttackDetails, Task> attacked = null,
        Func<Formation, DamageDetails, Task> damage = null,
        Func<Formation, DamageDetails, Task> damaged = null,
        Func<Formation, Task> killed = null,
        Func<Formation, Task> kill = null,
        Func<Formation, HealDetails, Task> heal = null,
        Func<Formation, HealDetails, Task> healed = null,
        Func<Formation, ArmorGainDetails, Task> armorGain = null,
        Func<Formation, ArmorGainDetails, Task> armorGained = null,
        Func<Formation, ArmorLoseDetails, Task> armorLose = null,
        Func<Formation, ArmorLoseDetails, Task> armorLost = null,
        Func<Formation, EvadeDetails, Task> evaded = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> buff = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> anyBuff = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> buffed = null,
        Tuple<int, Func<Formation, BuffDetails, Task<BuffDetails>>> anyBuffed = null,
        Func<Formation, ExhaustDetails, Task> consumed = null
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
        _startStage = startStage;
        _endStage = endStage;
        _startTurn = startTurn;
        _endTurn = endTurn;
        _startRound = startRound;
        _endRound = endRound;
        _startStep = startStep;
        _endStep = endStep;

        _attack = attack;
        _attacked = attacked;
        _damage = damage;
        _damaged = damaged;
        _killed = killed;
        _kill = kill;
        _heal = heal;
        _healed = healed;
        _armorGain = armorGain;
        _armorGained = armorGained;
        _armorLose = armorLose;
        _armorLost = armorLost;
        _evaded = evaded;

        _buff = buff;
        _anyBuff = anyBuff;
        _buffed = buffed;
        _anyBuffed = anyBuffed;

        _consumed = consumed;
    }
}
