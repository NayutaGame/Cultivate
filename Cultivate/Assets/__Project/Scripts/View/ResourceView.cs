
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PropagatePointer _propagatePointer;

    private Func<string> _textDelegate;
    private Func<string> _textHintDelegate;

    private void OnEnable()
    {
        _propagatePointer._onPointerEnter += PointerEnter;
        _propagatePointer._onPointerMove += PointerMove;
        _propagatePointer._onPointerExit += PointerExit;
    }

    private void OnDisable()
    {
        _propagatePointer._onPointerEnter -= PointerEnter;
        _propagatePointer._onPointerMove -= PointerMove;
        _propagatePointer._onPointerExit -= PointerExit;
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
    
    // emitter and attractor
}
