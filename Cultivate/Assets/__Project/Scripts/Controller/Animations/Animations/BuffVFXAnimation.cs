
using DG.Tweening;
using UnityEngine;

public class BuffVFXAnimation : Animation
{
    private IStageModel _model;
    private GameObject _prefab;

    private BuffVFXAnimation(IStageModel model, GameObject prefab, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _model = model;
        _prefab = prefab;
    }

    public static BuffVFXAnimation FromChangeStack(IStageModel model, BuffEntry entry, int diff, bool isAwait, bool induced)
        => new(model, diff > 0 ? GetPrefabFromGain(entry) : GetPrefabFromLose(entry), isAwait, induced);

    public static BuffVFXAnimation FromGainBuffDetails(GainBuffDetails d, bool isAwait)
        => new(d.Tgt.Model(), GetPrefabFromGain(d._buffEntry), isAwait, d.Induced);

    public static BuffVFXAnimation FromLoseBuffDetails(LoseBuffDetails d, bool isAwait)
        => new(d.Tgt.Model(), GetPrefabFromLose(d._buffEntry), isAwait, d.Induced);

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnBuffVFX()
    {
        GameObject gao = GameObject.Instantiate(_prefab, _model.VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private static GameObject GetPrefabFromGain(BuffEntry buffEntry)
    {
        if (buffEntry.GetName() == "灵气")
            return StageManager.Instance.LingQiVFXPrefab;

        if (buffEntry.Friendly)
            return StageManager.Instance.BuffVFXPrefab;
        
        return StageManager.Instance.DebuffVFXPrefab;
    }

    private static GameObject GetPrefabFromLose(BuffEntry buffEntry)
    {
        if (buffEntry.Friendly)
            return StageManager.Instance.DebuffVFXPrefab;
        
        return StageManager.Instance.BuffVFXPrefab;
    }
}
