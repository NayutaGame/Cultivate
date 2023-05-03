using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class StageAnimationDelegate : AnimationDelegate
{
    public TimelineView TimelineView;

    private Tween _tween;

    private float _speed;

    public StageAnimationDelegate()
    {
        _speed = 1;
    }

    public async Task PlayTween(TweenDescriptor descriptor)
    {
        if (descriptor is ShiftTweenDescriptor shift)
        {
            if (TimelineView != null)
            {
                Tween shiftTween = TimelineView.ShiftAnimation();
                await PlayTween(shiftTween);
            }
        }
        else if (descriptor is VfxTweenDescriptor vfx)
        {
            GameObject vfxGameObject = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, vfx.Slot.transform.position,
                Quaternion.identity, StageManager.Instance.VFXPool);
            vfxGameObject.GetComponent<FlowTextVFX>().Text.text = vfx.Text;
            Sequence vfxTween = DOTween.Sequence().AppendInterval(0.5f);
            await PlayTween(vfxTween);
        }
        else if (descriptor is AttackTweenDescriptor attack)
        {
            AttackDetails d = attack.AttackDetails;
            Sequence attackTween = DOTween.Sequence()
                .Append(d.Src.Slot().GetAttackTween())
                .Join(d.Tgt.Slot().GetAttackedTween())
                .AppendInterval(0.5f);
            await PlayTween(attackTween);
        }

        StageCanvas.Instance.Refresh();

        // case opening

        // bullet time at killing moment
        // gradually accelerating
        // speed control and skip
        // camera shake when large attacks
    }

    public async Task PlayTween(Tween tween)
    {
        _tween = tween;
        _tween.timeScale = _speed;
        _tween.SetAutoKill().Restart();
        await _tween.AsyncWaitForCompletion();
    }

    public void PauseTween()
    {
        _tween.Pause();
    }

    public void ResumeTween()
    {
        _tween.Play();
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        _tween.timeScale = _speed;
    }

    public void Skip()
    {
        // _tween.Complete();
        // Anim.Skip();
    }
}
