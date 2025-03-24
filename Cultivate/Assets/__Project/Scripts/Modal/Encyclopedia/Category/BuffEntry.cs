
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffEntry : Entry, IAnnotation
{
    public string GetName() => GetId();
    
    private string _description;
    public string GetDescription() => _description;

    public readonly BuffStackRule BuffStackRule;
    public readonly bool Friendly;
    public readonly bool Dispellable;
    public readonly bool IsForbiddenDebuff;

    private string _trivia;
    public string GetTrivia() => _trivia;

    [NonSerialized]
    public readonly StageClosure[] Closures;

    private SpriteEntry _spriteEntry;
    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite;

    /// <summary>
    /// 定义一个Buff
    /// </summary>
    /// <param name="id">名称</param>
    /// <param name="description">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="isForbiddenDebuff">是禁忌带来的debuff</param>
    /// <param name="trivia">趣闻</param>
    /// <param name="closures">事件捕获</param>
    public BuffEntry(
        string id,
        string description,
        BuffStackRule buffStackRule,
        bool friendly,
        bool dispellable,
        bool isForbiddenDebuff = false,
        string trivia = null,
        params StageClosure[] closures
    ) : base(id)
    {
        _description = description;
        BuffStackRule = buffStackRule;
        Friendly = friendly;
        Dispellable = dispellable;
        IsForbiddenDebuff = isForbiddenDebuff;
        _trivia = trivia;
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

    public WuXing? GetCorrespondingWuXing()
    {
        foreach (WuXing wuXing in WuXing.Traversal)
        {
            if (wuXing._elementaryBuff == GetName())
                return wuXing;
        }

        return null;
    }
}
