using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class StageReport
{
    private Sequence _seq;
    public Sequence Seq => _seq;
    private StringBuilder _sb;

    public int HomeLeftHp;
    public int AwayLeftHp;

    public void Append(string s)
        => _sb?.Append(s);

    public StageReport(Sequence seq = null, StringBuilder sb = null)
    {
        _seq = seq;
        _sb = sb;
    }

    public override string ToString()
    {
        return _sb?.ToString();
    }

    public void Play()
    {
    }

    // Sequence seq = DOTween.Sequence()
    //     .SetAutoKill()
    //     .OnComplete(() =>
    //     {
    //         Debug.Log("Animation is finished");
    //         // after animation is finished, exit
    //     });

    // animation
    // opening
    // turn all hero cards facing up

    // show card
    // act
    // card back

    // end

    // bullet time at killing moment
    // gradually accelerating
    // pause, speed control and skip
    // camera shake when large attacks

    // seq.Restart();
}
