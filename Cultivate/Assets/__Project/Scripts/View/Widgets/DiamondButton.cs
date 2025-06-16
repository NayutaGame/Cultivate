
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

    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron;
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron;

    [SerializeField] private Image HoverEffect1;
    [SerializeField] private Image HoverEffect2;
    [SerializeField] private Image PressEffect;

    protected override void AwakeFunction()
    {
        LeftClickNeuron = new();
        RightClickNeuron = new();
        base.AwakeFunction();
    }

    protected override Animator InitAnimator()
    {
        Animator animator = new(3);
        animator[ANY, IDLE] = EnterIdle;
        animator[ANY, HOVER] = EnterHover;
        animator[ANY, PRESS] = EnterPress;
        return animator;
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