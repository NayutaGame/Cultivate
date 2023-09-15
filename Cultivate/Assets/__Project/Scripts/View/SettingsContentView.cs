
using UnityEngine;

public class SettingsContentView : MonoBehaviour, IAddress
{
    public ListView Options;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();
    public virtual void SetAddress(Address address)
    {
        _address = address;

        Options.SetPrefabProvider(model =>
        {
            WidgetModel widgetModel = (WidgetModel)model;
            if (widgetModel is SliderModel)
                return 0;
            if (widgetModel is SwitchModel)
                return 1;
            if (widgetModel is CheckboxModel)
                return 2;
            if (widgetModel is ButtonModel)
                return 3;
            return -1;
        });
    }

    public virtual void Refresh()
    {
    }
}
