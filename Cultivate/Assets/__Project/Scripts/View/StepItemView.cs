
using UnityEngine;

public class StepItemView : AddressBehaviour
{
    [SerializeField] private ListView NodeListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        NodeListView.SetAddress(GetAddress().Append(".Nodes"));
    }

    public override void Refresh()
    {
        base.Refresh();

        NodeListView.Refresh();
    }
}
