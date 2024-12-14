
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectBehaviour : XBehaviour
{
    [SerializeField] private Image SelectionImage;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _animator = InitAnimator();
    }

    private Animator _animator;
    public Animator GetAnimator()
        => _animator;
    
    private Animator InitAnimator()
    {
        // 0 for unselected, 1 for selected
        Animator animator = new(3, "Select Behaviour");
        animator[-1, 0] = EnterUnselected;
        animator[-1, 1] = EnterSelected;
        animator.SetState(0);
        return animator;
    }
    
    private Tween EnterUnselected()
        => SelectionImage.DOFade(0, 0.15f);
    
    private Tween EnterSelected()
        => SelectionImage.DOFade(1, 0.15f);

    public async void SetSelectAsync(bool value)
    {
        await GetAnimator().SetStateAsync(value ? 1 : 0);
    }

    public void SetSelect(bool value)
    {
        GetAnimator().SetState(value ? 1 : 0);
    }
}
