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
            SpawnFlowText(vfx);

            Sequence waitTween = DOTween.Sequence().AppendInterval(0.5f);
            await PlayTween(waitTween);
        }
        else if (descriptor is AttackTweenDescriptor attack)
        {
            AttackDetails d = attack.AttackDetails;
            Sequence attackTween = DOTween.Sequence()
                .Append(GetAttackTween(d))
                .Join(GetAttackedTween(d))
                .AppendInterval(0.5f);
            await PlayTween(attackTween);
        }
        else if (descriptor is HealTweenDescriptor heal)
        {
            HealDetails d = heal.HealDetails;
            Sequence healTween = DOTween.Sequence()
                .Append(GetHealedTween(d))
                .AppendInterval(0.5f);
            await PlayTween(healTween);
        }
        else if (descriptor is BuffTweenDescriptor buff)
        {
            BuffDetails d = buff.BuffDetails;
            Sequence buffTween = DOTween.Sequence()
                .Append(GetBuffedTween(d))
                .AppendInterval(0.5f);
            await PlayTween(buffTween);
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

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }

    private Tween GetAttackTween(AttackDetails d)
    {
        Transform slotTransform = d.Src.Slot().transform;
        Transform entityTransform = d.Src.Slot().EntityTransform;
        int orient = -(d.Src.Index * 2 - 1);

        return DOTween.Sequence().SetAutoKill()
            .Append(DOTween.Sequence()
                .AppendInterval(0.1f)
                .AppendCallback(() => SpawnPiercingVFX(d)))
            .Join(entityTransform.DOMove(slotTransform.position + Vector3.right * orient, 0.2f).SetEase(Ease.InBack))
            .AppendCallback(StageCanvas.Instance.Refresh)
            .Append(entityTransform.DOMove(slotTransform.position, 0.3f).SetEase(Ease.OutQuad));
    }

    private Tween GetAttackedTween(AttackDetails d)
    {
        Transform entityTransform = d.Tgt.Slot().EntityTransform;
        int orient = -(d.Src.Index * 2 - 1);

        return DOTween.Sequence().SetDelay(0.2f).SetAutoKill()
            .AppendCallback(() => SpawnHitVFX(d))
            .Append(entityTransform.DOShakeRotation(0.6f, 10 * orient * Vector3.back, 10, 90, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InQuad));
    }

    private Tween GetHealedTween(HealDetails d)
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => SpawnHealVFX(d));
    }

    private Tween GetBuffedTween(BuffDetails d)
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => SpawnBuffVFX(d));
    }

    private void SpawnFlowText(VfxTweenDescriptor vfx)
    {
        GameObject flowTextGameObject = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, vfx.Slot.transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        flowTextGameObject.GetComponent<FlowTextVFX>().Text.text = vfx.Text;
    }

    private void SpawnPiercingVFX(AttackDetails d)
    {
        int orient = -(d.Src.Index * 2 - 1);
        GameObject gao = GameObject.Instantiate(StageManager.Instance.PiercingVFXFromWuXing[d.WuXing ?? WuXing.Jin], d.Src.Slot().transform.position + 2 * orient * Vector3.right,
            Quaternion.Euler(0, d.Src.Index * 180, 0), StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(d.Value));
        vfx.Play();
    }

    private void SpawnHitVFX(AttackDetails d)
    {
        int orient = -(d.Src.Index * 2 - 1);
        GameObject gao = GameObject.Instantiate(StageManager.Instance.HitVFXFromWuXing[d.WuXing ?? WuXing.Jin], d.Tgt.Slot().transform.position + -0.5f * orient * Vector3.right,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(d.Value));
        vfx.Play();
    }

    private void SpawnHealVFX(HealDetails d)
    {
        GameObject gao = GameObject.Instantiate(StageManager.Instance.HealVFXPrefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(d.Value));
        vfx.Play();
    }

    private void SpawnBuffVFX(BuffDetails d)
    {
        GameObject prefab = d._buffEntry.Friendly
            ? StageManager.Instance.BuffVFXPrefab
            : StageManager.Instance.DebuffVFXPrefab;
        GameObject gao = GameObject.Instantiate(prefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }
}
