
using CLLibrary;
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class CLButton : XView
{
    public enum ButtonState
    {
        Idle,
        Hover,
        Press,
        Inactive,
    }
    
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

    public void SetStateToActiveIf(bool active)
        => SetState(active ? ButtonState.Idle : ButtonState.Inactive);

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
    
    private void UpdateInteractable()
    {
        bool interactable = _state != ButtonState.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }

    protected abstract void JoinIdleTween(Sequence seq);
    protected abstract void JoinHoverTween(Sequence seq);
    protected abstract void JoinPressTween(Sequence seq);
    protected abstract void JoinInactiveTween(Sequence seq);
}
