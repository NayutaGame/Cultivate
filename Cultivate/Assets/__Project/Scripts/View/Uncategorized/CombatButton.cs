
using System;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public RectTransform _target;
    [SerializeField] public Image IconPlaceHolder;
    [SerializeField] public Sprite[] Icons;

    [HideInInspector] public Neuron<PointerEventData> LeftClickNeuron = new();
    [HideInInspector] public Neuron<PointerEventData> RightClickNeuron = new();

    public void Configure(int index)
    {
        IconPlaceHolder.sprite = Icons[index];
        UpdateState();
    }

    private bool _isAttractive;
    public void SetAttractive(bool value)
    {
        if (_isAttractive == value)
            return;
        
        _isAttractive = value;
        UpdateState();
    }
    
    private bool _isHover;
    public void SetHover(bool value)
    {
        if (_isHover == value)
            return;

        _isHover = value;
        UpdateState();
    }

    private Tween _handle;

    private void UpdateState()
    {
        int index = (_isAttractive ? 1 : 0) + (_isHover ? 2 : 0);
        
        _handle?.Kill();
        _handle = TweenSelector[index](_target);
        _handle.SetAutoKill();
        _handle.Restart();
    }

    public void OnPointerEnter(PointerEventData eventData)
        => SetHover(true);

    public void OnPointerExit(PointerEventData eventData)
        => SetHover(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            LeftClickNeuron.Invoke(eventData);
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            RightClickNeuron.Invoke(eventData);
        }
    }

    private static Func<RectTransform, Tween>[] TweenSelector = new Func<RectTransform, Tween>[4]
    {
        IdleTween, AttractiveTween, HoverTween, AttractiveHoverTween
    };

    private static Tween IdleTween(RectTransform target)
        => TweenAnimation.Jump(target);
    
    private static Tween AttractiveTween(RectTransform target)
        => TweenAnimation.Beats(target);

    private static Tween HoverTween(RectTransform target)
        => target.DOScale(1.2f * Vector3.one, 0.2f).SetEase(Ease.OutQuad);

    private static Tween AttractiveHoverTween(RectTransform target)
        => TweenAnimation.HarshBeats(target);
}
