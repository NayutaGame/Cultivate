
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button4StateSeperateContent : XView
{
    public enum ButtonState
    {
        Idle,
        Hover,
        Press,
        Inactive,
    }

    [SerializeField] private GameObject IdleContent;
    [SerializeField] private GameObject HoverContent;
    [SerializeField] private GameObject PressedContent;
    [SerializeField] private GameObject InactiveContent;

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
        seq.AppendCallback(() => IdleContent.SetActive(true));
        seq.AppendCallback(() => HoverContent.SetActive(false));
        seq.AppendCallback(() => PressedContent.SetActive(false));
        seq.AppendCallback(() => InactiveContent.SetActive(false));
    }

    protected virtual void JoinHoverTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(false));
        seq.AppendCallback(() => HoverContent.SetActive(true));
        seq.AppendCallback(() => PressedContent.SetActive(false));
        seq.AppendCallback(() => InactiveContent.SetActive(false));
    }

    protected virtual void JoinPressTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(false));
        seq.AppendCallback(() => HoverContent.SetActive(false));
        seq.AppendCallback(() => PressedContent.SetActive(true));
        seq.AppendCallback(() => InactiveContent.SetActive(false));
    }

    protected virtual void JoinInactiveTween(Sequence seq)
    {
        seq.AppendCallback(() => IdleContent.SetActive(false));
        seq.AppendCallback(() => HoverContent.SetActive(false));
        seq.AppendCallback(() => PressedContent.SetActive(false));
        seq.AppendCallback(() => InactiveContent.SetActive(true));
    }
    
    private void UpdateInteractable()
    {
        bool interactable = _state != ButtonState.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }
}