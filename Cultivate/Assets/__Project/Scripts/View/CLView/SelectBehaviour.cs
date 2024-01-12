
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CLView))]
public class SelectBehaviour : MonoBehaviour
{
    private CLView CLView;
    public SimpleView GetSimpleView() => CLView.GetSimpleView();

    public void Init(CLView clView)
    {
        CLView = clView;
    }

    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public bool IsSelected() => _selected;
    public void SetSelected(bool selected)
    {
        _selected = selected;
        AnimateSelect(SelectionImage, _selected);
    }

    private Tween _selectHandle;

    private void AnimateSelect(Image target, bool value)
    {
        _selectHandle?.Kill();
        _selectHandle = target.DOFade(value ? 1 : 0, 0.15f);
        _selectHandle.Restart();
    }
}
