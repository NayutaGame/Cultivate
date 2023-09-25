
using UnityEngine.EventSystems;

public interface CLSelectable : IPointerClickHandler
{
    bool IsSelected();
    void SetSelected(bool selected);
}
