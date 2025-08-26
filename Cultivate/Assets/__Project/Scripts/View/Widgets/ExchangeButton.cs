
using System;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExchangeButton : XView
{
    public enum ExchangeButtonState
    {
        Idle,
        Hover,
        Inactive,
    }
    
    [SerializeField] private Image ShowWhenInactive;
    [SerializeField] private Image HideWhenInactive;
    [SerializeField] private Image[] ShowWhenHover;

    private ExchangeButtonState _state;
    private Sequence _handle;
    
    public Neuron<InteractBehaviour, PointerEventData> OnClickNeuron = new();

    private void OnEnable()
    {
        _interactBehaviour.LeftClickNeuron.Add(OnClickNeuron);
        _interactBehaviour.PointerEnterNeuron.Add(SetStateToHover);
        _interactBehaviour.PointerExitNeuron.Add(SetStateToIdle);
    }

    private void OnDisable()
    {
        _interactBehaviour.LeftClickNeuron.Remove(OnClickNeuron);
        _interactBehaviour.PointerEnterNeuron.Remove(SetStateToHover);
        _interactBehaviour.PointerExitNeuron.Remove(SetStateToIdle);
    }

    public ExchangeButtonState GetState()
        => _state;

    public void SetState(ExchangeButtonState state)
    {
        _state = state;
        UpdateAnimation();
        UpdateInteractable();
    }

    private void SetStateToIdle(InteractBehaviour ib, PointerEventData d)
        => SetState(ExchangeButtonState.Idle);

    private void SetStateToHover(InteractBehaviour ib, PointerEventData d)
        => SetState(ExchangeButtonState.Hover);

    private void UpdateInteractable()
    {
        bool interactable = _state != ExchangeButtonState.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }

    private void UpdateAnimation()
    {
        _handle?.Kill();
        _handle = DOTween.Sequence();
        
        switch (_state)
        {
            case ExchangeButtonState.Idle:
                JoinIdleTween(_handle);
                break;
            case ExchangeButtonState.Hover:
                JoinHoverTween(_handle);
                break;
            case ExchangeButtonState.Inactive:
                JoinInactiveTween(_handle);
                break;
        }

        _handle.SetAutoKill();
        _handle.Restart();
    }

    private void JoinIdleTween(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(0, 0.15f))
            .Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }

    private void JoinHoverTween(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(0, 0.15f))
            .Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(1, 0.15f));
        }
    }

    private void JoinInactiveTween(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(1, 0.15f))
            .Join(HideWhenInactive.DOFade(0, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }
}
