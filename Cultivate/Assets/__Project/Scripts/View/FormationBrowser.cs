using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationBrowser : ListView // FormationGroupView
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
        interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, (v, d) => SelectFormation(v, d));
        SetDelegate(interactDelegate);
    }

    private bool SelectFormation(IInteractable view, PointerEventData eventData)
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
        return true;
    }
}
