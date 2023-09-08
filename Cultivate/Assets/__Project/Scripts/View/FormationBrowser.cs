using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBrowser : ListView<FormationGroupView>
{
    [SerializeField] private FormationGroupView _detailedGroupView;
    private FormationGroupView _selection;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
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
