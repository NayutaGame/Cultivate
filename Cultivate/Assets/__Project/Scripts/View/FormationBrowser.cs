using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBrowser : InventoryView<FormationView>
{
    public FormationView DetailedView;

    private FormationView _selection;

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
        _selection = (FormationView)view;
        if (_selection != null)
        {
            DetailedView.Configure(view.GetIndexPath());
            DetailedView.Refresh();
            _selection.SetSelected(true);
        }
        return true;
    }
}
