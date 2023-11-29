
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityBarView : EntityView
{
    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public virtual bool IsSelected() => _selected;
    public virtual void SetSelected(bool selected)
    {
        _selected = selected;
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    }
}
