
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    [SerializeField] private Image Image;

    public Tween ShowAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(() => Image.gameObject.SetActive(true))
            .Append(Image.DOFade(1, 0.3f).SetEase(Ease.OutQuad));
    }

    public Tween HideAnimation()
    {
        return DOTween.Sequence()
            .Append(Image.DOFade(0, 0.3f).SetEase(Ease.InQuad))
            .AppendCallback(() => Image.gameObject.SetActive(false));
    }

    private Tween _handle;

    public async Task PlayShowAnimation()
    {
        _handle?.Kill();
        _handle = ShowAnimation();
        _handle.SetAutoKill().Restart();
        await _handle.AsyncWaitForCompletion();
    }

    public async Task PlayHideAnimation()
    {
        _handle?.Kill();
        _handle = HideAnimation();
        _handle.SetAutoKill().Restart();
        await _handle.AsyncWaitForCompletion();
    }
}
