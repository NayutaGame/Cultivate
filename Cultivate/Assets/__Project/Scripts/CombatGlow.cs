
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CombatGlow : MonoBehaviour
{
    [SerializeField] private Image Glow;
    private Tween _handle;
    
    private void OnEnable()
    {
        _handle?.Kill();
        _handle = DOTween.Sequence()
            .Append(Glow.DOFade(1f, 0.15f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(0.2f, 0.15f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(1f, 0.15f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(0.2f, 0.15f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(1f, 0.15f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(1f, .6f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(0.2f, .6f).SetEase(Ease.InOutQuad))
            .Append(Glow.DOFade(1f, .6f).SetEase(Ease.InOutQuad))
            .SetLoops(99999, LoopType.Restart);
        _handle.SetAutoKill().Restart();
    }

    private void OnDisable()
    {
        _handle?.Kill();
    }
}