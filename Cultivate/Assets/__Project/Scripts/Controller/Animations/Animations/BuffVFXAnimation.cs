
using DG.Tweening;
using UnityEngine;

public class BuffVFXAnimation : Animation
{
    private StageEntity _target;
    private GameObject _prefab;

    private BuffVFXAnimation(bool isAwait, StageEntity target, GameObject prefab, bool induced) : base(isAwait, induced)
    {
        _target = target;
        _prefab = prefab;
    }

    public static BuffVFXAnimation FromGainBuffDetails(bool isAwait, GainBuffDetails d)
        => new(isAwait, d.Tgt, GetPrefab(d._buffEntry), d.Induced);

    public static BuffVFXAnimation FromChangeStack(bool isAwait, StageEntity target, BuffEntry entry, int diff, bool induced)
        => new(isAwait, target, diff > 0 ? GetPrefab(entry) : GetPrefabFromLose(entry), induced);

    public static BuffVFXAnimation FromLoseBuffDetails(bool isAwait, LoseBuffDetails d)
        => new(isAwait, d.Tgt, GetPrefabFromLose(d._buffEntry), d.Induced);

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnBuffVFX()
    {
        GameObject gao = GameObject.Instantiate(_prefab, _target.Model().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private static GameObject GetPrefab(BuffEntry buffEntry)
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
