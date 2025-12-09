
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : XView
{
    private bool _isHover;
    private bool _isDown;
    
    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();

    private Sequence _hoverHandle;
    private Sequence _downHandle;

    [SerializeField] private Image[] HoverImages;
    [SerializeField] private Image[] DownImages;

    [SerializeField] private TMP_Text Text;

    [SerializeField] private Color UpColor;
    [SerializeField] private Color DownColor;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        GetInteractBehaviour().LeftClickNeuron.Add(LeftClickNeuron);
    }

    protected virtual void OnEnable()
    {
        _interactBehaviour.PointerEnterNeuron.Add(SetHover);
        _interactBehaviour.PointerExitNeuron.Add(SetUnhover);
        
        _interactBehaviour.PointerEnterNeuron.Add(AudioManager.PlayButtonHover);
        _interactBehaviour.LeftClickNeuron.Add(AudioManager.PlayButtonPress);
    }

    protected virtual void OnDisable()
    {
        _interactBehaviour.PointerEnterNeuron.Remove(SetHover);
        _interactBehaviour.PointerExitNeuron.Remove(SetUnhover);
        
        _interactBehaviour.PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
        _interactBehaviour.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
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

    private void UpdateState()
    {
        UpdateHoverAnimation();
        UpdateDownAnimation();
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

        _downHandle.Join(Text.DOColor(color, 0.15f));

        _downHandle.SetAutoKill().Restart();
    }
}
