using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class StageReport : GDictionary
{
    private StageTimeline _timeline;
    public StageTimeline Timeline => _timeline;

    private StringBuilder _sb;

    public int HomeLeftHp;
    public int AwayLeftHp;

    private bool _homeVictory;
    public bool HomeVictory
    {
        get => _homeVictory;
        set
        {
            _homeVictory = value;
            if (value)
            {
                Append($"主场胜利\n");
            }
            else
            {
                Append($"主场失败\n");
            }
        }
    }

    public int MingYuanPenalty;

    private bool _useTween;
    public bool UseTween => _useTween;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public StageReport(bool useTween = false, bool useTimeline = false, bool useSb = false)
    {
        _accessors = new()
        {
            { "Timeline",              () => _timeline },
        };

        _useTween = useTween;

        if (useTimeline)
        {
            _timeline = new StageTimeline();
        }

        if (useSb)
        {
            _sb = new StringBuilder();
        }
    }

    public void Append(string s)
        => _sb?.Append(s);

    public void AppendNote(int entityIndex, StageSkill skill)
        => _timeline?.AppendNote(entityIndex, skill);

    public async Task PlayTween(TweenDescriptor descriptor)
    {
        if (!_useTween)
            return;
        await StageManager.Instance.Anim.PlayTween(descriptor);
    }

    public async Task PlayTween(Tween tween)
    {
        if (!_useTween)
            return;
        await StageManager.Instance.Anim.PlayTween(tween);
    }

    public override string ToString()
    {
        return _sb?.ToString();
    }
}
