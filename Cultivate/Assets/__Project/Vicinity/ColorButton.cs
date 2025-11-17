
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorButton : XView
{
    public enum State
    {
        Idle,
        Hover,
        Press,
        Inactive,
    }

    [SerializeField] private Image ColorImage;
    [SerializeField] public TMP_Text Text;
    
    private State _state;
    private Sequence _handle;

    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _interactBehaviour.LeftClickNeuron.Join(LeftClickNeuron);
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

    public State GetState()
        => _state;

    private void SetState(State state)
    {
        _state = state;
        UpdateAnimation();
        UpdateInteractable();
    }

    public void SetStateToInactiveFrom(bool shouldInactive)
        => SetState(shouldInactive ? State.Inactive : State.Idle);

    private void SetStateToIdle(InteractBehaviour ib, PointerEventData d)
        => SetState(State.Idle);

    private void SetStateToHover(InteractBehaviour ib, PointerEventData d)
        => SetState(State.Hover);

    private void SetStateToPress(InteractBehaviour ib, PointerEventData d)
        => SetState(State.Press);

    private void UpdateAnimation()
    {
        _handle?.Kill();
        _handle = DOTween.Sequence();
        
        switch (_state)
        {
            case State.Idle:
                JoinIdleTween(_handle);
                break;
            case State.Hover:
                JoinHoverTween(_handle);
                break;
            case State.Press:
                JoinPressTween(_handle);
                break;
            case State.Inactive:
                JoinInactiveTween(_handle);
                break;
        }

        _handle.SetAutoKill();
        _handle.Restart();
    }

    protected virtual void JoinIdleTween(Sequence seq)
    {
        seq.Join(ColorImage.DOColor(new Color(1, 1, 1, 0.05f), 0.15f));
    }

    protected virtual void JoinHoverTween(Sequence seq)
    {
        seq.Join(ColorImage.DOColor(new Color(1, 1, 1, 0.3f), 0.15f));
    }

    protected virtual void JoinPressTween(Sequence seq)
    {
        seq.Join(ColorImage.DOColor(new Color(1, 1, 1, 0.6f), 0.15f));
    }

    protected virtual void JoinInactiveTween(Sequence seq)
    {
        seq.Join(ColorImage.DOColor(new Color(1, 0, 0, 0.6f), 0.15f));
    }
    
    private void UpdateInteractable()
    {
        bool interactable = _state != State.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }
}