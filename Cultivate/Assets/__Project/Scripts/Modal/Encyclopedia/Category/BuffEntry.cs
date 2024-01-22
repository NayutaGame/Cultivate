
using System.Collections.Generic;
using CLLibrary;

public class BuffEntry : Entry, IAnnotation
{
    private string _description;
    public string Description => _description;

    private string _trivia;

    private IAnnotation[] _annotations;
    public IAnnotation[] GetAnnotations() => _annotations;

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
    /// <param name="name">名称</param>
    /// <param name="description">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="eventDescriptors">事件捕获</param>
    public BuffEntry(string name, string description, BuffStackRule buffStackRule, bool friendly, bool dispellable, string trivia = null,
        params StageEventDescriptor[] eventDescriptors
    ) : base(name)
    {
        _description = description;
        _trivia = trivia;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        _buffStackRule = buffStackRule;
        _friendly = friendly;
        _dispellable = dispellable;

        _eventDescriptorDict = new Dictionary<int, StageEventDescriptor>();
        foreach (var eventDescriptor in eventDescriptors)
            _eventDescriptorDict[eventDescriptor.EventId] = eventDescriptor;
    }

    public static implicit operator BuffEntry(string name) => Encyclopedia.BuffCategory[name];

    public void Generate()
    {
        string description = Description;

        List<IAnnotation> annotations = new();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!description.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!description.Contains(buffEntry.GetName()))
                continue;

            IAnnotation duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetAnnotatedDescription(string evaluated = null)
    {
        string toRet = evaluated ?? _description;
        foreach (var annotation in _annotations)
            toRet = toRet.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return toRet;
    }

    public string GetTrivia()
        => _trivia;
}
