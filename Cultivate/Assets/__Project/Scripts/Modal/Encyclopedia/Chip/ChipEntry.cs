using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class ChipEntry : Entry, IAnnotation
{
    private CLLibrary.Range _jingJieRange;
    public CLLibrary.Range JingJieRange => _jingJieRange;

    private ChipDescription _description;

    private IAnnotation[] _annotations;
    public IAnnotation[] GetAnnotations() => _annotations;

    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;

    private Sprite _cardFace;
    public Sprite CardFace
    {
        get
        {
            if (_cardFace != null)
                return _cardFace;

            _cardFace = _wuXing.HasValue ? CanvasManager.Instance.CardFaces[_wuXing.Value] : null;
            return _cardFace;
        }
    }

    private Func<Tile, RunChip, bool> _canPlug;
    private Action<Tile, RunChip> _plug;
    private Func<AcquiredRunChip, bool> _canUnplug;
    private Action<AcquiredRunChip> _unplug;

    public ChipEntry(string name,
        CLLibrary.Range jingJieRange,
        ChipDescription description,
        WuXing? wuXing,
        Func<Tile, RunChip, bool> canPlug,
        Action<Tile, RunChip> plug,
        Func<AcquiredRunChip, bool> canUnplug,
        Action<AcquiredRunChip> unplug
        ) : base(name)
    {
        _jingJieRange = jingJieRange;
        _description = description;
        _wuXing = wuXing;
        _canPlug = canPlug;
        _plug = plug;
        _canUnplug = canUnplug;
        _unplug = unplug;
    }

    public bool CanPlug(Tile tile, RunChip runChip) => _canPlug(tile, runChip);
    public void Plug(Tile tile, RunChip runChip) => _plug(tile, runChip);
    public bool CanUnplug(AcquiredRunChip acquiredRunChip) => _canUnplug(acquiredRunChip);
    public void Unplug(AcquiredRunChip acquiredRunChip) => _unplug(acquiredRunChip);

    public static implicit operator ChipEntry(string name) => Encyclopedia.ChipCategory[name];

    public string Evaluate(int j, int dj) => _description.Eval(0, j, dj, null);

    public string GetName()
        => Name;

    public void Generate()
    {
        string evaluated = Evaluate(0, 0);

        List<IAnnotation> annotations = new();

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (evaluated.Contains(buffEntry.Name))
                annotations.Add(buffEntry);
        }

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (evaluated.Contains(keywordEntry.Name))
                annotations.Add(keywordEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetAnnotatedDescription(string evaluated = null)
    {
        string toRet = evaluated ?? Evaluate(0, 0);
        foreach (var annotation in _annotations)
            toRet = toRet.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return toRet;
    }
}
