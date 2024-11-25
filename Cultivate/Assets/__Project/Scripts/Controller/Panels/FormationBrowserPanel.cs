
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationBrowserPanel : Panel
{
    public LegacyListView FormationBrowser;

    [SerializeField] private FormationGroupDetailedView _detailedGroupView;
    private FormationGroupBarView _selection;

    public override void Configure()
    {
        base.Configure();
        FormationBrowser.SetAddress(new Address("App.FormationInventory"));
        FormationBrowser.LeftClickNeuron.Add(SelectFormation);
    }

    public override void Refresh()
    {
        base.Refresh();
        FormationBrowser.Refresh();
    }

    private void SelectFormation(LegacyInteractBehaviour ib, PointerEventData eventData)
    {
        FormationGroupBarView view = ib.GetSimpleView() as FormationGroupBarView;
        if (view == null)
            return;

        if (_selection != null)
            _selection.SetSelected(false);
        _selection = view;
        if (_selection != null)
        {
            _detailedGroupView.SetAddress(ib.GetSimpleView().GetAddress());
            _detailedGroupView.Refresh();
            _selection.SetSelected(true);
        }
    }
}
