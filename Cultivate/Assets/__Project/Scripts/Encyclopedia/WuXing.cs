
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class WuXing : Entry
{
    [NonSerialized] private string _rawDescription;
    [NonSerialized] private Description _description;
    public Description GetDescription() => _description;
    
    [NonSerialized] private int _index;
    [NonSerialized] private bool _isBasic;
    [NonSerialized] private string _elementaryBuffName;
    [NonSerialized] private BuffEntry _elementaryBuff;
    [NonSerialized] private WuXing _prevWuXing;
    [NonSerialized] private WuXing _nextWuXing;
    [NonSerialized] private SpriteEntry _mergeSprite;
    [NonSerialized] private SpriteEntry _icon;
    [NonSerialized] private SpriteEntry _deco;
    
    public WuXing(
        string id,
        string name,
        string rawDescription,
        int index,
        bool isBasic,
        string elementaryBuffName) : base(id, name)
    {
        _rawDescription = rawDescription;
        _index = index;
        _isBasic = isBasic;
        _elementaryBuffName = elementaryBuffName;
    }

    public override void Init()
    {
        if (_isBasic)
        {
            _elementaryBuff = Encyclopedia.BuffCategory.FromName(_elementaryBuffName);
            int prevIndex = (_index + 4) % 5;
            _prevWuXing = Encyclopedia.WuXingCategory[prevIndex];
            int nextIndex = (_index + 1) % 5;
            _nextWuXing = Encyclopedia.WuXingCategory[nextIndex];

            _mergeSprite = Encyclopedia.SpriteCategory.FromName($"{GetName()}合成");
            _icon = Encyclopedia.SpriteCategory.FromName($"Tag{GetName()}");
            _deco = Encyclopedia.SpriteCategory.FromName($"WuXingDeco{GetName()}");
        }
        else
        {
            _mergeSprite = Encyclopedia.SpriteCategory.FromName("可以合成");
            _icon = Encyclopedia.SpriteCategory.FromName($"Tag{GetName()}");
            _deco = Encyclopedia.SpriteCategory.FromName($"WuXingDeco{GetName()}");
        }
        
        _description = new Description(_rawDescription);
    }

    public static int Length => 5;
    public int GetIndex() => _index;
    public bool IsBasic() => _isBasic;
    public BuffEntry GetElementaryBuff() => _elementaryBuff;
    public WuXing Next => _nextWuXing;
    public WuXing Prev => _prevWuXing;
    public Sprite GetMergeSprite() => _mergeSprite.Sprite;
    public Sprite GetIconSprite() => _icon.Sprite;
    public Sprite GetDecoSprite() => _deco.Sprite;
    
    public TagEntry GetTag()
        => TagEntry.FromIndex(_index);

    public Color GetColor()
    {
        return CanvasManager.Instance.WuXingColors[_index];
    }

    public GameObject GetHitVFXPrefab()
    {
        return StageManager.Instance.HitVFXFromWuXing[_index];
    }

    public GameObject GetPiercingVFXPrefab()
    {
        return StageManager.Instance.PiercingVFXFromWuXing[_index];
    }
    
    public static IEnumerable<WuXing> Traversal
    {
        get
        {
            foreach (WuXing item in Encyclopedia.WuXingCategory)
                yield return item;
        }
    }

    public static IEnumerable<WuXing> TraversalBasic
    {
        get
        {
            foreach (WuXing item in Encyclopedia.WuXingCategory)
            {
                if (item.IsBasic())
                    yield return item;
            }
        }
    }

    public static WuXing Jin => Encyclopedia.WuXingCategory[0];
    public static WuXing Shui => Encyclopedia.WuXingCategory[1];
    public static WuXing Mu => Encyclopedia.WuXingCategory[2];
    public static WuXing Huo => Encyclopedia.WuXingCategory[3];
    public static WuXing Tu => Encyclopedia.WuXingCategory[4];
    public static WuXing Wu => Encyclopedia.WuXingCategory[5];

    public static bool PredIsMatch(WuXingPred pred, WuXing wuXing)
    {
        if (pred == WuXingPred.任意)
            return true;
        
        if (wuXing == null)
            return pred == WuXingPred.无五行;
        
        switch (pred)
        {
            case WuXingPred.金:
                return wuXing.GetIndex() == 0;
            case WuXingPred.水:
                return wuXing.GetIndex() == 1;
            case WuXingPred.木:
                return wuXing.GetIndex() == 2;
            case WuXingPred.火:
                return wuXing.GetIndex() == 3;
            case WuXingPred.土:
                return wuXing.GetIndex() == 4;
            case WuXingPred.有五行:
                return wuXing.IsBasic();
            case WuXingPred.无五行:
                return !wuXing.IsBasic();
            default:
                return false;
        }
    }
    
    public static WuXingPred ToPred(WuXing wuXing)
    {
        if (wuXing == null)
            return WuXingPred.无五行;
        
        int index = wuXing.GetIndex();
        return index switch
        {
            0 => WuXingPred.金,
            1 => WuXingPred.水,
            2 => WuXingPred.木,
            3 => WuXingPred.火,
            4 => WuXingPred.土,
            _ => WuXingPred.无五行
        };
    }

    public static WuXing FromIndex(int index)
        => Encyclopedia.WuXingCategory[index];

    public static bool XiangSheng(WuXing lhs, WuXing rhs)
        => lhs != null && rhs != null && (lhs.Next == rhs || lhs.Prev == rhs);

    public static WuXing XiangShengNext(WuXing lhs, WuXing rhs)
    {
        if (lhs == null || rhs == null)
            return null;
        if (lhs.Next == rhs)
            return rhs.Next;
        if (rhs.Next == lhs)
            return lhs.Next;
        return null;
    }
}