using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FlowTextVFX : MonoBehaviour
{
    public TMP_Text Text;

    void Start()
    {
        Text.DOFade(0, 1f).SetEase(Ease.InQuad)
            .SetAutoKill()
            .Restart();
        transform.DOMove(transform.position + Vector3.up, 1f).SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(gameObject))
            .SetAutoKill()
            .Restart();
    }
}
