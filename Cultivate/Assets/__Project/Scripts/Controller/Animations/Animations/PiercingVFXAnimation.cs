
using DG.Tweening;
using Spine;
using UnityEngine;
using Random = System.Random;

public class PiercingVFXAnimation : Animation
{
    private AttackDetails _attackDetails;

    public PiercingVFXAnimation(bool isAwait, AttackDetails attackDetails) : base(isAwait, attackDetails.Induced)
    {
        _attackDetails = attackDetails.ShallowClone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnPiercingVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnPiercingVFX()
    {
        StageEntity src = _attackDetails.Src;
        WuXing wuXing = _attackDetails.WuXing ?? WuXing.Jin;
        int value = _attackDetails.Value;

        int orient = -(src.Index * 2 - 1);
        
        GameObject vfxGameObject;
        
        if (src.Model() is SpineModel spineModel)
        {
            Bone bone = spineModel.Skeleton.Skeleton.FindBone("emitterBone");
            Vector3 pos = new Vector3(bone.WorldX * spineModel.Skeleton.transform.localScale.x, bone.WorldY * spineModel.Skeleton.transform.localScale.y, 0);

            Vector2 randomOffset;
            if (_attackDetails.Times <= 3)
            {
                randomOffset = Vector2.zero;
            }
            else
            {
                randomOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            }
            
            vfxGameObject = GameObject.Instantiate(StageManager.Instance.PiercingVFXFromWuXing[wuXing], src.Model().transform.position + pos + (Vector3)randomOffset,
                Quaternion.Euler(0, src.Index * 180, 0), StageManager.Instance.VFXPool);
        }
        else
        {
            vfxGameObject = GameObject.Instantiate(StageManager.Instance.PiercingVFXFromWuXing[wuXing], src.Model().VFXTransform.position + 2 * orient * Vector3.right,
                Quaternion.Euler(0, src.Index * 180, 0), StageManager.Instance.VFXPool);
        }
        
        VFX vfx = vfxGameObject.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(value));
        vfx.Play();
    }

    // private Tween GetAttackedTween()
    // {
    //     StageEntity src = _attackDetails.Src;
    //     StageEntity tgt = _attackDetails.Tgt;
    //
    //     Transform entityTransform = tgt.Slot().EntityTransform;
    //     int orient = -(src.Index * 2 - 1);
    //
    //     return entityTransform
    //         .DOShakeRotation(0.6f, 10 * orient * Vector3.back, 10, 90, true, ShakeRandomnessMode.Harmonic)
    //         .SetEase(Ease.InQuad);
    // }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }
}
