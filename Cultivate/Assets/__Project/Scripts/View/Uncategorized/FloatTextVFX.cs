
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatTextVFX : MonoBehaviour
{
    public TMP_Text Text;

    void Start()
    {
        Text.DOFade(0, 1f).SetEase(Ease.InQuad)
            .SetAutoKill().Restart();
        transform.DOMove(transform.position + 3f * Vector3.up, 1f).SetEase(Ease.OutCubic)
            .OnComplete(() => Destroy(gameObject))
            .SetAutoKill().Restart();
    }
}
