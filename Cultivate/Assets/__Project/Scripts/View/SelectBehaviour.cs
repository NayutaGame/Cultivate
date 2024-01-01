
using UnityEngine;
using UnityEngine.UI;

public class SelectBehaviour : MonoBehaviour
{
    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public bool IsSelected() => _selected;
    public void SetSelected(bool selected)
    {
        _selected = selected;
    }
}
