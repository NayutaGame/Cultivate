
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationBrowser : ListView
{
    [SerializeField] private FormationGroupDetailedView _detailedGroupView;
    private FormationGroupBarView _selection;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        ConfigureInteractDelegate();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate interactDelegate = new(1, getId: view =>
        {
            if (view is FormationGroupBarView)
                return 0;
            return null;
        });
        interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, SelectFormation);
        SetDelegate(interactDelegate);
    }

    private void SelectFormation(IInteractable view, PointerEventData eventData)
    {
        if (_selection != null)
            _selection.SetSelected(false);
        _selection = (FormationGroupBarView)view;
        if (_selection != null)
        {
            _detailedGroupView.SetAddress(view.GetAddress());
            _detailedGroupView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
