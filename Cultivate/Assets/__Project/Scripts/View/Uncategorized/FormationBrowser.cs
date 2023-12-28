
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
        InteractHandler interactHandler = new(1, getId: view =>
        {
            if (view.GetComponent<FormationGroupBarView>() != null)
                return 0;
            return null;
        });
        interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, SelectFormation);
        SetHandler(interactHandler);
    }

    private void SelectFormation(InteractBehaviour view, PointerEventData eventData)
    {
        if (_selection != null)
            _selection.SetSelected(false);
        _selection = view.GetComponent<FormationGroupBarView>();
        if (_selection != null)
        {
            _detailedGroupView.SetAddress(view.GetComponent<AddressBehaviour>().GetAddress());
            _detailedGroupView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
