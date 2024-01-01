
using DG.Tweening;
using UnityEngine;

public class AnimateBehaviour : MonoBehaviour
{
    public ComplexView ComplexView;





    private Tween _selectHandle;

    public void SetSelected(bool selected)
    {
        // _selected = selected;
        //
        // _selectHandle?.Kill();
        // _selectHandle = SelectionImage.DOFade(_selected ? 1 : 0, 0.15f);
        // _selectHandle.Restart();
    }







    private Tween _animationHandle;

    public void SetPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(ComplexView.GetDisplayTransform(), pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }

    public void SetPivotWithoutAnimation(RectTransform pivot)
    {
        _animationHandle?.Kill();
        ComplexView.SetDisplayTransform(pivot);
    }

    public void SetStartAndPivot(RectTransform start, RectTransform pivot)
    {
        SetPivotWithoutAnimation(start);
        SetPivot(pivot);
    }
}
