
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BreathingButton : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image Image;
    [SerializeField] private Image HoverImage;

    private bool _breathing;

    public Neuron<PointerEventData> ClickNeuron = new();

    private Tween _handle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_breathing)
            return;

        HoverImage.DOFade(1f, 0.1f).SetEase(Ease.OutQuad).SetAutoKill().Restart();

        _handle?.Kill();
        _handle = _rectTransform.DOScale(1.2f * Vector3.one, 0.2f).SetEase(Ease.OutQuad).SetAutoKill();
        _handle.Restart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_breathing)
            return;

        HoverImage.DOFade(0f, 0.1f).SetEase(Ease.InQuad).SetAutoKill().Restart();

        _handle?.Kill();
        _handle = _rectTransform.DOScale(Vector3.one, 0.7f).From(1.2f)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad).SetAutoKill();
        _handle.Restart();
    }

    public void OnPointerClick(PointerEventData eventData) => ClickNeuron.Invoke(eventData);

    public void SetBreathing(bool breathing)
    {
        if (_breathing == breathing)
            return;

        _breathing = breathing;
        Refresh();
    }

    private void Refresh()
    {
        _handle?.Kill();

        if (_breathing)
        {
            _handle = _rectTransform.DOScale(Vector3.one, 0.7f).From(1.2f)
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad).SetAutoKill();
        }
        else
        {
            _handle = _rectTransform.DOScale(Vector3.one, 0.7f)
                .SetEase(Ease.InQuad).SetAutoKill();
        }

        _handle.Restart();
    }

    public void SetSprite(Sprite sprite)
    {
        Image.sprite = sprite;
    }

    public void SetColor(Color c)
    {
        Image.DOKill();
        Image.DOColor(c, 0.1f).SetEase(Ease.InOutQuad).SetAutoKill().Restart();
    }
}
