
using System;
using System.Collections.Generic;

public class FormationEntry : IFormationModel, Addressable
{
    private FormationGroupEntry _formationGroupEntry;
    public FormationGroupEntry GetFormationGroupEntry() => _formationGroupEntry;
    public void SetFormationGroupEntry(FormationGroupEntry formationGroupEntry) => _formationGroupEntry = formationGroupEntry;

    public int GetOrder() => _formationGroupEntry.Order;

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;

    private string _rawRewardDescription;
    private Description _rewardDescription;
    
    public void GenerateDescription()
        => _rewardDescription = new Description(_rawRewardDescription);

    private string _trivia;
    public string GetTrivia() => _trivia;

    private int _requirement;
    public int GetRequirement() => _requirement;

    [NonSerialized] public readonly RunClosure[] RunClosures;
    [NonSerialized] public readonly StageClosure[] StageClosures;

    public ListModel<MarkModel> GetMarks() => _formationGroupEntry.GetMarks();

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Marks",                      thisObject => ((FormationEntry)thisObject).GetMarks() },
    };
    public object Get(string s) => Accessor[s](this);
    /// <summary>
    /// 定义一个Formation
    /// </summary>
    /// <param name="jingJie">境界</param>
    /// <param name="conditionDescription">条件的描述</param>
    /// <param name="rawRewardDescription">奖励的描述</param>
    /// <param name="runClosures">事件捕获</param>
    /// <param name="stageClosures">事件捕获</param>
    public FormationEntry(JingJie jingJie, string rawRewardDescription, string trivia, int requirement,
        RunClosure[] runClosures = null,
        params StageClosure[] stageClosures
    )
    {
        _jingJie = jingJie;
        _rawRewardDescription = rawRewardDescription;
        _trivia = trivia;
        _requirement = requirement;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        RunClosures = runClosures ?? Array.Empty<RunClosure>();
        StageClosures = stageClosures ?? Array.Empty<StageClosure>();
    }

    #region IFormationModel

    public string GetName() => _formationGroupEntry.GetName();
    public JingJie GetLowestJingJie() => _formationGroupEntry.GetLowestJingJie();
    public JingJie? GetActivatedJingJie() => _jingJie;
    public string GetConditionDescription() => _formationGroupEntry.GetConditionDescription();
    
    public Description GetRewardDescription() => _rewardDescription;
    public Description GetRewardDescription(JingJie jingJie)
        => _formationGroupEntry.GetRewardDescription(jingJie);

    public string GetTriviaFromJingJie(JingJie jingJie) => _formationGroupEntry.GetTriviaFromJingJie(jingJie);
    public JingJie GetIncrementedJingJie(JingJie jingJie) => _formationGroupEntry.GetIncrementedJingJie(jingJie);
    public int GetRequirementFromJingJie(JingJie jingJie) => _formationGroupEntry.GetRequirementFromJingJie(jingJie);
    public Predicate<ISkill> GetContributorPred() => _formationGroupEntry.GetContributorPred();
    public SpriteEntry GetSprite() => _formationGroupEntry.GetSprite();

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _formationGroupEntry.GetMin();
    public int GetMax() => _formationGroupEntry.GetMax();
    public int? GetValue() => null;
    public Address GetMarkListModelAddress(Address address)
        => address.Append(".Marks");

    #endregion
}
