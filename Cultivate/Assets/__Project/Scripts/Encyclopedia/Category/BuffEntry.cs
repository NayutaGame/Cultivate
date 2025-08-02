
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffEntry : Entry, AnnotatableBuff
{
    private string _rawDescription;
    private Description _description;
    public Description GetDescription() => _description;

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
    
    public bool CanShowAnnotation() => true;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    /// <summary>
    /// 定义一个Buff
    /// </summary>
    /// <param name="id">名称</param>
    /// <param name="rawDescription">描述</param>
    /// <param name="buffStackRule">堆叠规则</param>
    /// <param name="friendly">是否有益</param>
    /// <param name="dispellable">是否可驱散</param>
    /// <param name="isForbiddenDebuff">是禁忌带来的debuff</param>
    /// <param name="trivia">趣闻</param>
    /// <param name="closures">事件捕获</param>
    public BuffEntry(
        string id,
        string name,
        string rawDescription,
        BuffStackRule buffStackRule,
        bool friendly,
        bool dispellable,
        bool isForbiddenDebuff = false,
        string trivia = null,
        params StageClosure[] closures
    ) : base(id, name)
    {
        _rawDescription = rawDescription;
        BuffStackRule = buffStackRule;
        Friendly = friendly;
        Dispellable = dispellable;
        IsForbiddenDebuff = isForbiddenDebuff;
        _trivia = trivia;
        Closures = closures ?? Array.Empty<StageClosure>();
    }
    
    public override void Init()
    {
        _description = new Description(_rawDescription);
        CreateSprite();
    }

    public void CreateSprite()
    {
        string name = $"Buff{GetName()}";
        if (Encyclopedia.SpriteCategory.ContainsName(name))
        {
            _spriteEntry = Encyclopedia.SpriteCategory.FromName(name);
        }
        else
        {
            string id = $"SpriteAuto{Encyclopedia.SpriteCategory.Count():0000}";
            _spriteEntry = new(id, name, $"Images/BuffIcons/{GetName()}");
            Encyclopedia.SpriteCategory.Add(_spriteEntry);
        }
    }
}
