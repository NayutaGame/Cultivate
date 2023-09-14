using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsContentView : MonoBehaviour, IAddress
{
    // [SerializeField] private WidgetListView Widgets;

    [SerializeField] private GameObject SliderWidget;
    [SerializeField] private GameObject CheckboxWidget;
    [SerializeField] private GameObject ButtonWidget;
    [SerializeField] private GameObject SwitchWidget;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private SettingsContentModel _model;

    public void SetAddress(Address address)
    {
        _address = address;
        _model = Get<SettingsContentModel>();

        // _model.Widgets
    }

    public void Refresh()
    {
    }
}
