using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;
using UnityEngine.Analytics;

public class BuffEntry : Entry, IAnnotation
{
    private string _description;
    public string Description => _description;

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

    public Func<Buff, StageEntity, int, Task> _gain;
    public Func<Buff, StageEntity, Task> _lose;
    public Func<Buff, StageEntity, Task> _stackChanged;

    public Dictionary<string, StageEventCapture> _eventCaptureDict;

    /// <summary>
    /// 定义一个Buff
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="description">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="gain">获得时的额外行为</param>
    /// <param name="lose">失去时的额外行为</param>
    /// <param name="stackChanged">层数改变时的额外行为</param>
    /// <param name="eventCaptures">事件捕获</param>
    public BuffEntry(string name, string description, BuffStackRule buffStackRule, bool friendly, bool dispellable,
        Func<Buff, StageEntity, int, Task> gain = null,
        Func<Buff, StageEntity, Task> lose = null,
        Func<Buff, StageEntity, Task> stackChanged = null,
        params StageEventCapture[] eventCaptures
    ) : base(name)
    {
        _description = description;
        // _sprite = Resources.Load<Sprite>($"Sprites/Buff/{Name}");
        _buffStackRule = buffStackRule;
        _friendly = friendly;
        _dispellable = dispellable;

        _gain = gain;
        _lose = lose;
        _stackChanged = stackChanged;

        _eventCaptureDict = new Dictionary<string, StageEventCapture>();
        foreach (var stageEventCapture in eventCaptures)
            _eventCaptureDict[stageEventCapture.EventId] = stageEventCapture;
    }

    // public void ConfigureNote(StringBuilder sb)
    // {
    //     sb.Append($"<style=\"NoteName\">{Name}</style>\n");
    //     sb.Append($"<style=\"NoteSeparator\">__________</style>\n");
    //     sb.Append($"<style=\"NoteDescription\">{Description}</style>");
    // }

    public static implicit operator BuffEntry(string name) => Encyclopedia.BuffCategory[name];

    public string GetName()
        => Name;

    public void Generate()
    {
        string description = Description;

        List<IAnnotation> annotations = new();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!description.Contains(keywordEntry.Name))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!description.Contains(buffEntry.Name))
                continue;

            IAnnotation duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.Name);
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
}
