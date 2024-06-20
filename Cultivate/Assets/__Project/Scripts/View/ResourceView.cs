
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

    private int _unit;
    private BoundedInt _content;

    private Func<BoundedInt> _refreshDelegate;
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

    public void Configure(int unit, Func<BoundedInt> refreshDelegate, Func<string> hintDelegate)
    {
        _unit = unit;
        _refreshDelegate = refreshDelegate;
        _hintDelegate = hintDelegate;
    }

    public void Refresh()
    {
        _content = _refreshDelegate?.Invoke().Clone();
        _text.text = _content?.ToString();

        _gapStart = 0;
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

    public void Gain(Vector2 position, int value)
    {
        _emitterTransform.position = CanvasManager.Instance.UI2World(position);
        _emitter.Emit(new ParticleSystem.EmitParams(), value);
    }

    private Tween _expandHandle;

    private void OnAttracted()
    {
        _expandHandle?.Kill();

        _content.Curr += _unit;
        _text.text = _content.ToString();

        _gapStart++;

        if (_textTransform.localScale.x >= 1.5f)
            _textTransform.localScale = Vector3.one * 1.5f;
        
        _expandHandle = DOTween.Sequence()
            .Append(_textTransform.DOScale(2, 0.08f).SetEase(Ease.OutQuad))
            .Append(_textTransform.DOScale(1, 0.08f).SetEase(Ease.InQuad));
        _expandHandle.SetAutoKill().Restart();
    }

    private Tween _numberHandle;
    private int _gap;

    private int _contentStart;
    private int _gapStart;
    private float _gapT;

    [SerializeField] private TMP_Text _gapText;

    public void NumberChange(int value)
    {
        _gap += value;
        _gapText.text = _gap.ToString();
        
        _contentStart = _content.Curr;
        _gapStart = _gap;
        _gapT = 0;
        
        _numberHandle?.Kill();
        _numberHandle = DOTween.Sequence()
            .Append(_gapText.DOFade(1, 0.2f).SetEase(Ease.OutQuad))
            .Append(DOTween.To(GetGapT, SetGapT, 1, 1f).SetEase(Ease.OutQuad))
            .AppendInterval(0.2f)
            .Append(_gapText.DOFade(0, 0.2f).SetEase(Ease.InQuad));
        _numberHandle.SetAutoKill().Restart();
    }

    private float GetGapT() => _gapT;
    private void SetGapT(float value)
    {
        _gapT = value;
        _gap = (int) Mathf.Lerp(_gapStart, 0, _gapT);
        _gapText.text = _gap.ToString();
        _content.Curr = _contentStart + _gapStart - _gap;
        _text.text = _content.Curr.ToString();
    }

    public void NumberChangeNoAnimation(int value)
    {
        _content.Curr += value;
        _text.text = _content.ToString();

        _gapStart -= value;
    }
}
