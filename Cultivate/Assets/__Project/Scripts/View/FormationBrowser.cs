
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationBrowser : ListView
{
    [SerializeField] private FormationGroupView _detailedGroupView;
    private FormationGroupView _selection;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        ConfigureInteractDelegate();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate interactDelegate = new(1, getId: view => 0);
        interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, SelectFormation);
        SetDelegate(interactDelegate);
    }

    private void SelectFormation(IInteractable view, PointerEventData eventData)
    {
        if (_selection != null)
            _selection.SetSelected(false);
        _selection = (FormationGroupView)view;
        if (_selection != null)
        {
            _detailedGroupView.SetAddress(view.GetAddress());
            _detailedGroupView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
