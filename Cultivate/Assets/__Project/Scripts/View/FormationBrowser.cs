using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBrowser : ListView<FormationGroupView>
{
    [SerializeField] private FormationGroupView _detailedGroupView;
    private FormationGroupView _selection;

    public override void Configure(Address address)
    {
        base.Configure(address);
        ConfigureInteractDelegate();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate interactDelegate = new(1,
            getId: view => 0,
            lMouseTable: new Func<IInteractable, bool>[]
            {
                SelectFormation,
            }
        );
        SetDelegate(interactDelegate);
    }

    private bool SelectFormation(IInteractable view)
    {
        if (_selection != null)
            _selection.SetSelected(false);
        _selection = (FormationGroupView)view;
        if (_selection != null)
        {
            _detailedGroupView.Configure(view.GetIndexPath());
            _detailedGroupView.Refresh();
            _selection.SetSelected(true);
        }
        return true;
    }
}
