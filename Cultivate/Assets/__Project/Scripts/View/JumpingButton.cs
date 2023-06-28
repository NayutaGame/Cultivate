using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpingButton : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image Image;
    [SerializeField] private Image HoverImage;

    private bool _jumping;
    private event Action<PointerEventData> _onPointerClick;

    public void AddListener(Action<PointerEventData> listener)
    {
        _onPointerClick += listener;
    }

    public void RemoveAllListeners()
    {
        _onPointerClick = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_jumping)
        {
            HoverImage.DOFade(1f, 0.1f).SetEase(Ease.OutQuad).SetAutoKill().Restart();
            _rectTransform.DOKill();
            _rectTransform.DOScale(1.2f * Vector3.one, 0.2f).SetEase(Ease.OutQuad).SetAutoKill().Restart();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_jumping)
        {
            HoverImage.DOFade(0f, 0.1f).SetEase(Ease.InQuad).SetAutoKill().Restart();
            _rectTransform.DOKill();
            _rectTransform.DOScale(1f * Vector3.one, 0.7f).From(1.2f)
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad).SetAutoKill().Restart();
        }
    }

    public void OnPointerClick(PointerEventData eventData) => _onPointerClick?.Invoke(eventData);

    public void SetJumping(bool jumping)
    {
        _jumping = jumping;

        _rectTransform.DOKill();
        if (_jumping)
        {
            _rectTransform.DOScale(1f * Vector3.one, 0.7f).From(1.2f)
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad).SetAutoKill().Restart();
        }
        else
        {
            _rectTransform.DOScale(1f * Vector3.one, 0.7f)
                .SetEase(Ease.InQuad).SetAutoKill().Restart();
        }
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
