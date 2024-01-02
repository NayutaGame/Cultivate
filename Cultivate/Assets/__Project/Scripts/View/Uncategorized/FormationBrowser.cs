
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationBrowser : ListView
{
    [SerializeField] private FormationGroupDetailedView _detailedGroupView;
    private FormationGroupBarView _selection;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        LeftClickNeuron.Add(SelectFormation);
    }

    private void SelectFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        FormationGroupBarView view = ib.GetComponent<FormationGroupBarView>();
        if (view == null)
            return;

        if (_selection != null)
            _selection.SetSelected(false);
        _selection = view;
        if (_selection != null)
        {
            _detailedGroupView.SetAddress(ib.GetComponent<AddressBehaviour>().GetAddress());
            _detailedGroupView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
