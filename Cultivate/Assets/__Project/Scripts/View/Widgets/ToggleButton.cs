
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : XView
{
    private bool _isHover = false;
    private bool _isDown = false;
    private bool _isInteractable = true;
    
    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();

    private Sequence _hoverHandle;
    private Sequence _downHandle;
    private Sequence _interactableHandle;

    [SerializeField] private Image[] HoverImages;
    [SerializeField] private Image[] DownImages;
    [SerializeField] private Image[] InteractableImage;

    [SerializeField] private TMP_Text Text;

    [SerializeField] private Color UpColor;
    [SerializeField] private Color DownColor;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        GetInteractBehaviour().NeuronBundle.LeftClickNeuron.Add(LeftClickNeuron);
    }

    protected virtual void OnEnable()
    {
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Add(SetHover);
        _interactBehaviour.NeuronBundle.PointerExitNeuron.Add(SetUnhover);
        
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        _interactBehaviour.NeuronBundle.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
    }

    protected virtual void OnDisable()
    {
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Remove(SetHover);
        _interactBehaviour.NeuronBundle.PointerExitNeuron.Remove(SetUnhover);
        
        _interactBehaviour.NeuronBundle.PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        _interactBehaviour.NeuronBundle.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
    }

    private void SetHover(InteractBehaviour ib, PointerEventData d)
    {
        IsHover = true;
    }

    private void SetUnhover(InteractBehaviour ib, PointerEventData d)
    {
        IsHover = false;
    }

    public bool IsHover
    {
        get => _isHover;
        set
        {
            if (_isHover == value)
                return;
            _isHover = value;
            UpdateHoverAnimation();
        }
    }
    
    public bool IsDown
    {
        get => _isDown;
        set
        {
            if (_isDown == value)
                return;
            _isDown = value;
            UpdateDownAnimation();
        }
    }
    
    public bool IsInteractable
    {
        get => _isInteractable;
        set
        {
            if (_isInteractable == value)
                return;
            _isInteractable = value;
            _interactBehaviour.SetInteractable(_isInteractable);
            UpdateInteractableAnimation();
        }
    }

    private void UpdateState()
    {
        UpdateHoverAnimation();
        UpdateDownAnimation();
        UpdateInteractableAnimation();
    }

    private void UpdateHoverAnimation()
    {
        _hoverHandle?.Kill();
        _hoverHandle = DOTween.Sequence();

        int value = _isHover ? 1 : 0;
        foreach (Image image in HoverImages)
            _hoverHandle.Join(image.DOFade(value, 0.15f));

        _hoverHandle.SetAutoKill().Restart();
    }

    private void UpdateDownAnimation()
    {
        _downHandle?.Kill();
        _downHandle = DOTween.Sequence();

        int value = _isDown ? 1 : 0;
        Color color = _isDown ? DownColor : UpColor;
        
        foreach (Image image in DownImages)
            _downHandle.Join(image.DOFade(value, 0.15f));

        if (Text != null)
            _downHandle.Join(Text.DOColor(color, 0.15f));

        _downHandle.SetAutoKill().Restart();
    }

    private void UpdateInteractableAnimation()
    {
        _interactableHandle?.Kill();
        _interactableHandle = DOTween.Sequence();

        int value = _isInteractable ? 0 : 1;
        Color color = _isInteractable ? UpColor : DownColor;
        
        foreach (Image image in InteractableImage)
            _interactableHandle.Join(image.DOFade(value, 0.15f));

        if (Text != null)
            _interactableHandle.Join(Text.DOColor(color, 0.15f));

        _interactableHandle.SetAutoKill().Restart();
    }
}
