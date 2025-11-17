
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button4State : XView
{
    public enum ButtonState
    {
        Idle,
        Hover,
        Press,
        Inactive,
    }
    
    [SerializeField] private Image ShowWhenInactive;
    [SerializeField] private Image HideWhenInactive;
    [SerializeField] private Image[] ShowWhenHover;

    private ButtonState _state;
    private Sequence _handle;

    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _interactBehaviour.LeftClickNeuron.Join(LeftClickNeuron);
        _interactBehaviour.RightClickNeuron.Join(RightClickNeuron);
    }

    protected virtual void OnEnable()
    {
        _interactBehaviour.PointerEnterNeuron.Add(SetStateToHover);
        _interactBehaviour.PointerExitNeuron.Add(SetStateToIdle);
        _interactBehaviour.PointerDownNeuron.Add(SetStateToPress);
        _interactBehaviour.PointerUpNeuron.Add(SetStateToIdle);
        if (AudioManager.Instance != null)
        {
            _interactBehaviour.PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
            _interactBehaviour.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
            _interactBehaviour.RightClickNeuron.Add(AudioManager.PlayButtonPress);
        }
        
        UpdateAnimation();
        UpdateInteractable();
    }

    protected virtual void OnDisable()
    {
        _interactBehaviour.PointerEnterNeuron.Remove(SetStateToHover);
        _interactBehaviour.PointerExitNeuron.Remove(SetStateToIdle);
        _interactBehaviour.PointerDownNeuron.Remove(SetStateToPress);
        _interactBehaviour.PointerUpNeuron.Remove(SetStateToIdle);
        
        _interactBehaviour.PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        _interactBehaviour.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        _interactBehaviour.RightClickNeuron.Remove(AudioManager.PlayButtonPress);
    }

    public ButtonState GetState()
        => _state;

    private void SetState(ButtonState state)
    {
        _state = state;
        UpdateAnimation();
        UpdateInteractable();
    }

    public void SetStateToInactiveFrom(bool shouldInactive)
        => SetState(shouldInactive ? ButtonState.Inactive : ButtonState.Idle);

    private void SetStateToIdle(InteractBehaviour ib, PointerEventData d)
        => SetState(ButtonState.Idle);

    private void SetStateToHover(InteractBehaviour ib, PointerEventData d)
        => SetState(ButtonState.Hover);

    private void SetStateToPress(InteractBehaviour ib, PointerEventData d)
        => SetState(ButtonState.Press);

    private void UpdateAnimation()
    {
        _handle?.Kill();
        _handle = DOTween.Sequence();
        
        switch (_state)
        {
            case ButtonState.Idle:
                JoinIdleTween(_handle);
                break;
            case ButtonState.Hover:
                JoinHoverTween(_handle);
                break;
            case ButtonState.Press:
                JoinPressTween(_handle);
                break;
            case ButtonState.Inactive:
                JoinInactiveTween(_handle);
                break;
        }

        _handle.SetAutoKill();
        _handle.Restart();
    }

    protected virtual void JoinIdleTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(0, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(1, 0.15f));
        
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }

    protected virtual void JoinHoverTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(0, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(1, 0.15f));
        }
    }

    protected virtual void JoinPressTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(0.6f, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(1, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(1, 0.15f));
        }
    }

    protected virtual void JoinInactiveTween(Sequence seq)
    {
        if (ShowWhenInactive != null)
            seq.Join(ShowWhenInactive.DOFade(1, 0.15f));
        if (HideWhenInactive != null)
            seq.Join(HideWhenInactive.DOFade(0, 0.15f));
        foreach (Image image in ShowWhenHover)
        {
            seq.Join(image.DOFade(0, 0.15f));
        }
    }
    
    private void UpdateInteractable()
    {
        bool interactable = _state != ButtonState.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }
}