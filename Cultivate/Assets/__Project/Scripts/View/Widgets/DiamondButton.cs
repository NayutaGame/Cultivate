
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiamondButton : XView
{
    public static readonly int ANY = -1;
    public static readonly int IDLE = 0;
    public static readonly int HOVER = 1;
    public static readonly int PRESS = 2;
    
    [SerializeField] private GameObject Content;
    [SerializeField] private GameObject ContentNoninteractable;
    [SerializeField] private GameObject Frame;
    [SerializeField] public GameObject FrameNoninteractable;

    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron;
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron;

    [SerializeField] private Image HoverEffect1;
    [SerializeField] private Image HoverEffect2;
    [SerializeField] private Image PressEffect;

    private bool _isInteractable;
    public void SetInteractable(bool value)
    {
        _isInteractable = value;
        if (Content != null)
            Content.SetActive(_isInteractable);
        if (Frame != null)
            Frame.SetActive(_isInteractable);
        if (ContentNoninteractable != null)
            ContentNoninteractable.SetActive(!_isInteractable);
        if (FrameNoninteractable != null)
            FrameNoninteractable.SetActive(!_isInteractable);
        RefreshIb();
    }

    protected override void AwakeFunction()
    {
        LeftClickNeuron = new();
        RightClickNeuron = new();
        base.AwakeFunction();
        SetInteractable(true);
    }

    protected override Animator InitAnimator()
    {
        Animator animator = new(3);
        animator[ANY, IDLE] = EnterIdle;
        animator[ANY, HOVER] = EnterHover;
        animator[ANY, PRESS] = EnterPress;
        return animator;
    }

    private void RefreshIb()
    {
        InteractBehaviour ib = GetInteractBehaviour();
        if (ib != null)
            InitInteractBehaviour(ib);
        
        if (_isInteractable)
        {
            ib.PointerEnterNeuron.Join(PointerEnter);
            ib.PointerExitNeuron.Join(PointerExit);
            ib.PointerDownNeuron.Join(PointerDown);
            ib.PointerUpNeuron.Join(PointerUp);
            if (AudioManager.Instance != null)
            {
                ib.PointerEnterNeuron.Join(AudioManager.PlayButtonHover);
                ib.LeftClickNeuron.Join(AudioManager.PlayButtonPress);
                ib.RightClickNeuron.Join(AudioManager.PlayButtonPress);
            }
        
            ib.LeftClickNeuron.Join(LeftClickNeuron);
            ib.RightClickNeuron.Join(RightClickNeuron);
        }
        else
        {
            ib.PointerEnterNeuron.Remove(PointerEnter);
            ib.PointerExitNeuron.Remove(PointerExit);
            ib.PointerDownNeuron.Remove(PointerDown);
            ib.PointerUpNeuron.Remove(PointerUp);
            if (AudioManager.Instance != null)
            {
                ib.PointerEnterNeuron.Remove(AudioManager.PlayButtonHover);
                ib.LeftClickNeuron.Remove(AudioManager.PlayButtonPress);
                ib.RightClickNeuron.Remove(AudioManager.PlayButtonPress);
            }
        
            ib.LeftClickNeuron.Remove(LeftClickNeuron);
            ib.RightClickNeuron.Remove(RightClickNeuron);
        }
    }

    protected override void InitInteractBehaviour(InteractBehaviour ib)
    {
        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
        ib.PointerDownNeuron.Join(PointerDown);
        ib.PointerUpNeuron.Join(PointerUp);
        if (AudioManager.Instance != null)
        {
            ib.PointerEnterNeuron.Join(AudioManager.PlayButtonHover);
            ib.LeftClickNeuron.Join(AudioManager.PlayButtonPress);
            ib.RightClickNeuron.Join(AudioManager.PlayButtonPress);
        }
        
        ib.LeftClickNeuron.Join(LeftClickNeuron);
        ib.RightClickNeuron.Join(RightClickNeuron);
    }

    private Tween EnterIdle()
        => DOTween.Sequence()
            .Append(HoverEffect1.DOFade(0, 0.15f))
            .Join(HoverEffect2.DOFade(0, 0.15f))
            .Join(PressEffect.DOFade(0, 0.15f));

    private Tween EnterHover()
        => DOTween.Sequence()
            .Append(HoverEffect1.DOFade(1, 0.15f))
            .Join(HoverEffect2.DOFade(1, 0.15f))
            .Join(PressEffect.DOFade(0, 0.15f));

    private Tween EnterPress()
        => DOTween.Sequence()
            .Append(HoverEffect1.DOFade(0, 0.15f))
            .Join(HoverEffect2.DOFade(0, 0.15f))
            .Join(PressEffect.DOFade(1, 0.15f));
    
    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(HOVER);
    }
    
    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }

    private void PointerDown(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(PRESS);
    }

    private void PointerUp(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }
}