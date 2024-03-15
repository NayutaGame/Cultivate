
using System.Collections.Generic;

public class BuffEntry : Entry, IAnnotation
{
    public string GetName() => GetId();
    
    private string _description;
    public string GetDescription() => _description;

    private string _trivia;
    public string GetTrivia() => _trivia;

    // private Sprite _sprite;
    // public Sprite Sprite => _sprite;

    private BuffStackRule _buffStackRule;
    public BuffStackRule BuffStackRule => _buffStackRule;

    private bool _friendly;
    public bool Friendly => _friendly;

    private bool _dispellable;
    public bool Dispellable => _dispellable;

    public Dictionary<int, StageEventDescriptor> _eventDescriptorDict;

    /// <summary>
    /// 定义一个Buff
    /// </summary>
    /// <param name="id">名称</param>
    /// <param name="description">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="eventDescriptors">事件捕获</param>
    public BuffEntry(string id, string description, BuffStackRule buffStackRule, bool friendly, bool dispellable, string trivia = null,
        params StageEventDescriptor[] eventDescriptors
    ) : base(id)
    {
        _description = description;
        _trivia = trivia;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        _buffStackRule = buffStackRule;
        _friendly = friendly;
        _dispellable = dispellable;

        _eventDescriptorDict = new Dictionary<int, StageEventDescriptor>();
        if (eventDescriptors != null)
            foreach (var eventDescriptor in eventDescriptors)
                _eventDescriptorDict[eventDescriptor.EventId] = eventDescriptor;
    }
    
    private AnnotationArray _annotationArray;
    public void GenerateAnnotations()
        => _annotationArray = AnnotationArray.FromDescription(GetDescription());
    public string GetHighlight(string description)
        => _annotationArray.HighlightFromDescription(description);
    public string GetHighlight()
        => GetHighlight(GetDescription());
    
    public string GetExplanation()
        => _annotationArray.GetExplanation();

    public static implicit operator BuffEntry(string id) => Encyclopedia.BuffCategory[id];
}
