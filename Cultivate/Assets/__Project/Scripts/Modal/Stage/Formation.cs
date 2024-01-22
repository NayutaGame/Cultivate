
/// <summary>
/// Formation
/// </summary>
public class Formation : StageEventListener, IFormationModel
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private RunFormation _runFormation;

    public FormationEntry GetEntry() => _runFormation.GetEntry();

    public StageEventDict _eventDict;

    public Formation(StageEntity owner, RunFormation runFormation)
    {
        _owner = owner;
        _runFormation = runFormation;

        _eventDict = new();
    }

    public void Register()
    {
        foreach (int eventId in GetEntry()._eventDescriptorDict.Keys)
        {
            StageEventDescriptor eventDescriptor = GetEntry()._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == StageEventDict.STAGE_ENVIRONMENT)
                _owner.Env.EventDict.Register(this, eventDescriptor);
            else if (senderId == StageEventDict.STAGE_ENTITY)
                ;
            else if (senderId == StageEventDict.STAGE_BUFF)
                ;
            else if (senderId == StageEventDict.STAGE_FORMATION)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    public void Unregister()
    {
        foreach (int eventId in GetEntry()._eventDescriptorDict.Keys)
        {
            StageEventDescriptor eventDescriptor = GetEntry()._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == StageEventDict.STAGE_ENVIRONMENT)
                _owner.Env.EventDict.Unregister(this, eventDescriptor);
            else if (senderId == StageEventDict.STAGE_ENTITY)
                ;
            else if (senderId == StageEventDict.STAGE_BUFF)
                ;
            else if (senderId == StageEventDict.STAGE_FORMATION)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }

    #region IFormationModel

    public string GetName() => _runFormation.GetName();
    public JingJie GetJingJie() => _runFormation.GetJingJie();
    public string GetConditionDescription() => _runFormation.GetConditionDescription();
    public string GetRewardDescriptionFromJingJie(JingJie jingJie) => _runFormation.GetRewardDescriptionFromJingJie(jingJie);
    public int? GetProgress() => _runFormation.GetProgress();
    public string GetTriviaFromJingJie(JingJie jingJie) => _runFormation.GetTriviaFromJingJie(jingJie);

    #endregion

    #region IMarkedSliderModel

    public int GetMin() => _runFormation.GetMin();
    public int GetMax() => _runFormation.GetMax();
    public int GetValue() => _runFormation.GetValue();

    #endregion
}
