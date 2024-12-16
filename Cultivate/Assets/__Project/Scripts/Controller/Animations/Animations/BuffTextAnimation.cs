
using DG.Tweening;
using UnityEngine;

public class BuffTextAnimation : Animation
{
    private StageEntity _target;
    private string _text;

    private BuffTextAnimation(bool isAwait, StageEntity target, string text, bool induced) : base(isAwait, induced)
    {
        _target = target;
        _text = text;
    }
    
    public static BuffTextAnimation FromGainBuffDetails(bool isAwait, GainBuffDetails d)
        => new(isAwait, d.Tgt, $"{d._buffEntry.GetName()} +{d._stack}", d.Induced);

    public static BuffTextAnimation FromChangeStack(bool isAwait, StageEntity target, BuffEntry entry, int diff, bool induced)
        => new(isAwait, target, $"{entry.GetName()} {diff}", induced);

    public static BuffTextAnimation FromLoseBuffDetails(bool isAwait, LoseBuffDetails d)
        => new(isAwait, d.Tgt, $"{d._buffEntry.GetName()} -{d._stack}", d.Induced);

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        GameObject gao = GameObject.Instantiate(StageManager.Instance.FloatTextVFXPrefab, _target.Model().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        gao.GetComponent<FloatTextVFX>().Text.text = _text;
    }
}
