
using UnityEngine;

public class MenuView : XView
{
    [SerializeField] public ListView ListView;
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        // MenuDetails d = Get<MenuDetails>();
        ListView.SetAddress(address.Append(".Options"));
    }
}