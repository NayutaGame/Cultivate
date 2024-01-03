
using UnityEngine;
using UnityEngine.UI;

public class SelectBehaviour : MonoBehaviour
{
    public ComplexView ComplexView;

    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public bool IsSelected() => _selected;
    public void SetSelected(bool selected)
    {
        _selected = selected;

        AnimateBehaviour animateBehaviour = ComplexView.GetAnimateBehaviour();
        if (animateBehaviour == null)
            return;

        animateBehaviour.AnimateSelect(SelectionImage, _selected);
    }
}
