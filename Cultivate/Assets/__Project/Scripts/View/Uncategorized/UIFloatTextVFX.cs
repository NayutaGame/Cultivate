
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIFloatTextVFX : MonoBehaviour
{
    public RectTransform _transform;
    public TMP_Text Text;

    void Start()
    {
        _transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(_transform.DOScale(3, 0.2f).SetEase(Ease.InCubic))
            .Append(_transform.DOScale(1, 0.8f).SetEase(Ease.OutCubic))
            .SetAutoKill().Restart();
        Text.DOFade(0, 1f).SetEase(Ease.InQuad)
            .SetAutoKill().Restart();
        _transform.DOAnchorPos(_transform.anchoredPosition + 300 * Vector2.up, 1f).SetEase(Ease.OutCubic)
            .OnComplete(() => Destroy(gameObject))
            .SetAutoKill().Restart();
    }
}
