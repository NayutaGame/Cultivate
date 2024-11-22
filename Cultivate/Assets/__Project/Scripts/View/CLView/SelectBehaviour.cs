
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(XView))]
public class SelectBehaviour : MonoBehaviour
{
    private XView _xView;
    public SimpleView GetSimpleView() => _xView.GetSimpleView();

    public void Init(XView xView)
    {
        _xView = xView;
    }

    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public bool IsSelected() => _selected;
    public void SetSelected(bool selected, bool animated = true)
    {
        _selected = selected;
        if (animated)
            AnimateSelect(SelectionImage, _selected);
        else
            NonAnimateSelect(SelectionImage, _selected);
    }

    private Tween _selectHandle;

    private void AnimateSelect(Image target, bool value)
    {
        _selectHandle?.Kill();
        _selectHandle = target.DOFade(value ? 1 : 0, 0.15f);
        _selectHandle.Restart();
    }

    private void NonAnimateSelect(Image target, bool value)
    {
        target.color = new Color(target.color.r, target.color.g, target.color.b, value ? 1 : 0);
    }
}
