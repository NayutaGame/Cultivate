
using DG.Tweening;
using Spine;
using UnityEngine;

public class PiercingVFXAnimation : Animation
{
    private IStageModel _model;
    private AttackDetails _attackDetails;

    public PiercingVFXAnimation(AttackDetails attackDetails, bool isAwait) : base(isAwait, attackDetails.Induced)
    {
        _model = attackDetails.Src.Model();
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
        
        if (_model is SpineModel spineModel)
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
            
            vfxGameObject = GameObject.Instantiate(GetPrefab(wuXing), _model.transform.position + pos + (Vector3)randomOffset,
                Quaternion.Euler(0, src.Index * 180, 0), StageManager.Instance.VFXPool);
        }
        else
        {
            vfxGameObject = GameObject.Instantiate(GetPrefab(wuXing), _model.VFXTransform.position + 2 * orient * Vector3.right,
                Quaternion.Euler(0, src.Index * 180, 0), StageManager.Instance.VFXPool);
        }
        
        VFX vfx = vfxGameObject.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }

    private GameObject GetPrefab(WuXing wuXing)
    {
        return StageManager.Instance.PiercingVFXFromWuXing[wuXing];
    }
}
