
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

    private BoundedInt _content;

    private Func<BoundedInt> _refreshDelegate;
    private Action<BoundedInt> _attractDelegate;
    private Func<string> _hintDelegate;

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

    public void Configure(Func<BoundedInt> refreshDelegate, Action<BoundedInt> attractDelegate, Func<string> hintDelegate)
    {
        _refreshDelegate = refreshDelegate;
        _attractDelegate = attractDelegate;
        _hintDelegate = hintDelegate;
    }

    public void Refresh()
    {
        _content = _refreshDelegate?.Invoke().Clone();
        _text.text = _content?.ToString();
    }

    private void PointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText(_hintDelegate?.Invoke());
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
        _emitter.Emit(new ParticleSystem.EmitParams(), value);
    }

    private Tween _handle;

    private void OnAttracted()
    {
        _handle?.Kill();

        _attractDelegate?.Invoke(_content);
        _text.text = _content?.ToString();

        if (_textTransform.localScale.x >= 1.5f)
            _textTransform.localScale = Vector3.one * 1.5f;
        
        _handle = DOTween.Sequence()
            .Append(_textTransform.DOScale(2, 0.08f).SetEase(Ease.OutQuad))
            .Append(_textTransform.DOScale(1, 0.08f).SetEase(Ease.InQuad));
        _handle.SetAutoKill().Restart();
    }
}
