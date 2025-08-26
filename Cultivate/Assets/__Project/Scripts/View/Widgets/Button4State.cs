
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button4State : XView
{
    public enum DiamondButtonState
    {
        Idle,
        Hover,
        Press,
        Inactive,
    }
    
    [SerializeField] private Image ShowWhenInactive;
    [SerializeField] private Image HideWhenInactive;
    [SerializeField] private Image[] ShowWhenHover;

    private DiamondButtonState _state;
    private Sequence _handle;

    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();

    private void OnEnable()
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
        
        _interactBehaviour.LeftClickNeuron.Add(LeftClickNeuron);
        _interactBehaviour.RightClickNeuron.Add(RightClickNeuron);
    }

    private void OnDisable()
    {
        _interactBehaviour.PointerEnterNeuron.Remove(SetStateToHover);
        _interactBehaviour.PointerExitNeuron.Remove(SetStateToIdle);
        _interactBehaviour.PointerDownNeuron.Remove(SetStateToPress);
        _interactBehaviour.PointerUpNeuron.Remove(SetStateToIdle);
        
        _interactBehaviour.PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        _interactBehaviour.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
        _interactBehaviour.RightClickNeuron.Remove(AudioManager.PlayButtonPress);
        
        _interactBehaviour.LeftClickNeuron.Remove(LeftClickNeuron);
        _interactBehaviour.RightClickNeuron.Remove(RightClickNeuron);
    }

    public DiamondButtonState GetState()
        => _state;

    public void SetState(DiamondButtonState state)
    {
        _state = state;
        UpdateAnimation();
        UpdateInteractable();
    }

    private void SetStateToIdle(InteractBehaviour ib, PointerEventData d)
        => SetState(DiamondButtonState.Idle);

    private void SetStateToHover(InteractBehaviour ib, PointerEventData d)
        => SetState(DiamondButtonState.Hover);

    private void SetStateToPress(InteractBehaviour ib, PointerEventData d)
        => SetState(DiamondButtonState.Press);

    private void UpdateAnimation()
    {
        _handle?.Kill();
        _handle = DOTween.Sequence();
        
        switch (_state)
        {
            case DiamondButtonState.Idle:
                JoinIdleTween(_handle);
                break;
            case DiamondButtonState.Hover:
                JoinHoverTween(_handle);
                break;
            case DiamondButtonState.Press:
                JoinPressTween(_handle);
                break;
            case DiamondButtonState.Inactive:
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

    private void JoinPressTween(Sequence seq)
    {
        seq.Join(ShowWhenInactive.DOFade(0.6f, 0.15f))
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
    
    private void UpdateInteractable()
    {
        bool interactable = _state != DiamondButtonState.Inactive;
        _interactBehaviour.SetInteractable(interactable);
    }
}