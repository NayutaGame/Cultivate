
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffEntry : Entry, IAnnotation
{
    public string GetName() => GetId();
    
    private string _description;
    public string GetDescription() => _description;

    private string _trivia;
    public string GetTrivia() => _trivia;

    private SpriteEntry _spriteEntry;
    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite;

    public readonly BuffStackRule BuffStackRule;
    public readonly bool Friendly;
    public readonly bool Dispellable;

    [NonSerialized]
    public readonly StageClosure[] Closures;

    /// <summary>
    /// 定义一个Buff
    /// </summary>
    /// <param name="id">名称</param>
    /// <param name="description">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="trivia">趣闻</param>
    /// <param name="closures">事件捕获</param>
    public BuffEntry(string id, string description, BuffStackRule buffStackRule, bool friendly, bool dispellable, string trivia = null,
        params StageClosure[] closures
    ) : base(id)
    {
        _description = description;
        _trivia = trivia;
        BuffStackRule = buffStackRule;
        Friendly = friendly;
        Dispellable = dispellable;
        Closures = closures ?? Array.Empty<StageClosure>();
        _spriteEntry = $"Buff{GetName()}";
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
