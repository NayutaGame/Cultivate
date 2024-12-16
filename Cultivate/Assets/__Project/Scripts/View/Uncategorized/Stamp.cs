
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Stamp : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Image _image;
    
    private Tween _handle;

    private void OnEnable()
    {
        _handle = DOTween.Sequence().SetAutoKill()
            .Append(_image.DOFade(1, 0.15f).From(0).SetEase(Ease.InQuad))
            .Append(_transform.DOScale(1, 0.15f).From(1.5f).SetEase(Ease.InQuad));
        _handle.Restart();
        
        // AudioManager.Play("Stamp");
    }

    private void OnDisable()
    {
        _handle?.Kill();
    }
}
