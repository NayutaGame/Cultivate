using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class StageAnimationDelegate : AnimationDelegate
{
    public TimelineView TimelineView;

    public async Task PlayTween(TweenDescriptor descriptor)
    {
        if (descriptor is ShiftTweenDescriptor shift)
        {
            if (TimelineView != null)
                await TimelineView.ShiftAnimation();
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
        // pause, speed control and skip
        // camera shake when large attacks
    }

    public async Task PlayTween(Tween tween)
    {
        tween.SetAutoKill().Restart();
        await tween.AsyncWaitForCompletion();
    }
}
