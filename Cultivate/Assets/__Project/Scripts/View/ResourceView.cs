
using System;
using Coffee.UIExtensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private RectTransform _textTransform;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PropagatePointer _propagatePointer;
    [SerializeField] private RectTransform _emitterTransform;
    [SerializeField] private ParticleSystem _emitter;
    [SerializeField] private UIParticleAttractor _attractor;

    private Func<string> _textDelegate;
    private Func<string> _textHintDelegate;

    private void OnEnable()
    {
        _propagatePointer._onPointerEnter += PointerEnter;
        _propagatePointer._onPointerMove += PointerMove;
        _propagatePointer._onPointerExit += PointerExit;
        _attractor.onAttracted.AddListener(OnAttracted);
    }

    private void OnDisable()
    {
        _propagatePointer._onPointerEnter -= PointerEnter;
        _propagatePointer._onPointerMove -= PointerMove;
        _propagatePointer._onPointerExit -= PointerExit;
        _attractor.onAttracted.RemoveListener(OnAttracted);
    }

    public void Configure(Func<string> textDelegate, Func<string> textHintDelegate)
    {
        _textDelegate = textDelegate;
        _textHintDelegate = textHintDelegate;
    }

    public void Refresh()
    {
        _text.text = _textDelegate?.Invoke();
    }

    private void PointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText(_textHintDelegate?.Invoke());
    }

    private void PointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText(null);
    }

    private void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.UpdateMousePos(eventData.position);
    }

    public void Emit(Vector2 position, int value)
    {
        _emitterTransform.position = CanvasManager.Instance.UI2World(new Vector2(Screen.width / 2, Screen.height / 2));
        _emitter.Emit(new ParticleSystem.EmitParams(), value * 20);
    }

    private Tween _handle;

    private void OnAttracted()
    {
        _handle?.Kill();

        if (_textTransform.localScale.x >= 1.5f)
            _textTransform.localScale = Vector3.one * 1.5f;
        
        _handle = DOTween.Sequence()
            .Append(_textTransform.DOScale(2, 0.08f).SetEase(Ease.OutQuad))
            .Append(_textTransform.DOScale(1, 0.08f).SetEase(Ease.InQuad));
        _handle.SetAutoKill().Restart();
    }
}
