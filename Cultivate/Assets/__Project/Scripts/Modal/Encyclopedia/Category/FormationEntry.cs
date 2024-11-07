
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

    private string _rewardDescription;
    public string GetRewardDescription() => _rewardDescription;
    public string GetHighlightedRewardDescription() => GetHighlight();
    public string GetRewardDescriptionAnnotation() => _annotationArray.GetExplanation();
    
    private AnnotationArray _annotationArray;
    public void GenerateAnnotations()
        => _annotationArray = AnnotationArray.FromDescription(GetRewardDescription());
    public string GetHighlight(string description)
        => _annotationArray.HighlightFromDescription(description);
    public string GetHighlight()
        => GetHighlight(GetRewardDescription());

    private string _trivia;
    public string GetTrivia() => _trivia;

    private int _requirement;
    public int GetRequirement() => _requirement;

    [NonSerialized]
    public readonly StageClosure[] Closures;

    public ListModel<MarkModel> GetMarks() => _formationGroupEntry.GetMarks();

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    /// <summary>
    /// 定义一个Formation
    /// </summary>
    /// <param name="jingJie">境界</param>
    /// <param name="conditionDescription">条件的描述</param>
    /// <param name="rewardDescription">奖励的描述</param>
    /// <param name="closures">事件捕获</param>
    public FormationEntry(JingJie jingJie, string rewardDescription, string trivia, int requirement,
        params StageClosure[] closures
    )
    {
        _accessors = new Dictionary<string, Func<object>>()
        {
            { "Marks", GetMarks },
        };

        _jingJie = jingJie;
        _rewardDescription = rewardDescription;
        _trivia = trivia;
        _requirement = requirement;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        Closures = closures;
    }

    #region IFormationModel

    public string GetName() => _formationGroupEntry.GetName();
    public JingJie GetLowestJingJie() => _formationGroupEntry.GetLowestJingJie();
    public JingJie? GetActivatedJingJie() => _jingJie;
    public string GetConditionDescription() => _formationGroupEntry.GetConditionDescription();
    public string GetRewardDescriptionFromJingJie(JingJie jingJie) => _formationGroupEntry.GetRewardDescriptionFromJingJie(jingJie);

    public string GetHighlightedRewardDescriptionFromJingJie(JingJie jingJie)
        => _formationGroupEntry.GetHighlightedRewardDescriptionFromJingJie(jingJie);

    public string GetRewardDescriptionAnnotationFromJingJie(JingJie jingJie)
        => _formationGroupEntry.GetRewardDescriptionAnnotationFromJingJie(jingJie);

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
