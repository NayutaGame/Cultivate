
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(XView))]
public class FlipBehaviour : XBehaviour
{
    [SerializeField] private GameObject FrontContent;
    [SerializeField] private SlotView FrontSlot;
    [SerializeField] private GameObject BackContent;
    [SerializeField] private SlotView BackSlot;
    
    private Tween _handle;

    private bool _flipped;

    public void SetFlipped(bool flipped)
    {
        if (_flipped == flipped) return;
        _flipped = flipped;

        _handle?.Kill();

        if (_flipped)
        {
            // 正面 -> 反面
            _handle = DOTween.Sequence()
                .AppendCallback(() => FrontSlot.GrabberRelease())
                .Append(GetView().GetRect().DOLocalRotate(new Vector3(0, 90f, 0), 0.15f).SetEase(Ease.InQuad))
                .AppendCallback(() =>
                {
                    FrontContent.SetActive(false);
                    BackContent.SetActive(true);
                    GetView().GetRect().localEulerAngles = new Vector3(0, -90f, 0);
                })
                .Append(GetView().GetRect().DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.OutQuad));
        }
        else
        {
            // 反面 -> 正面
            _handle = DOTween.Sequence()
                .AppendCallback(() => BackSlot.GrabberRelease())
                .Append(GetView().GetRect().DOLocalRotate(new Vector3(0, -90f, 0), 0.15f).SetEase(Ease.InQuad))
                .AppendCallback(() =>
                {
                    FrontContent.SetActive(true);
                    BackContent.SetActive(false);
                    GetView().GetRect().localEulerAngles = new Vector3(0, 90f, 0);
                })
                .Append(GetView().GetRect().DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.OutQuad));
        }
        _handle.SetAutoKill().Restart();
    }

    private void OnDestroy()
    {
        _handle?.Kill();
    }
}
