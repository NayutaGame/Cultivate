
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExchangeButton : XView
{
    public enum ButtonState
    {
        Idle,
        Hover,
        Inactive,
    }
    
    [SerializeField] private Image ShowWhenInactive;
    [SerializeField] private Image HideWhenInactive;
    [SerializeField] private Image[] ShowWhenHover;

    private ButtonState _state;
    private Sequence _handle;
    
    public Neuron<InteractBehaviour, PointerEventData> OnClickNeuron = new();

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _interactBehaviour.LeftClickNeuron.Join(OnClickNeuron.Invoke);
        _interactBehaviour.PointerEnterNeuron.Join(SetStateToHover);
        _interactBehaviour.PointerExitNeuron.Join(SetStateToIdle);
        
    }

    public ButtonState GetState()
        => _state;

    private void SetStateToIdle(InteractBehaviour ib, PointerEventData d)
        => SetState(ButtonState.Idle);

    private void SetStateToHover(InteractBehaviour ib, PointerEventData d)
        => SetState(ButtonState.Hover);

    public void SetState(ButtonState state)
    {
        _state = state;
        UpdateAnimation();
        UpdateInteractable();
    }

    private void UpdateInteractable()
    {
        bool interactable = _state != ButtonState.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }

    private void UpdateAnimation()
    {
        _handle?.Kill();
        _handle = DOTween.Sequence();
        
        switch (_state)
        {
            case ButtonState.Idle:
                AppendIdle(_handle);
                break;
            case ButtonState.Hover:
                AppendHover(_handle);
                break;
            case ButtonState.Inactive:
                AppendInactive(_handle);
                break;
        }

        _handle.SetAutoKill();
        _handle.Restart();
    }

    private void AppendIdle(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(0, 0.15f))
            .Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }

    private void AppendHover(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(0, 0.15f))
            .Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(1, 0.15f));
        }
    }

    private void AppendInactive(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(1, 0.15f))
            .Join(HideWhenInactive.DOFade(0, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }
}
