
using UnityEngine;
using UnityEngine.UI;

public class RoomIconView : SimpleView
{
    [SerializeField] private Image Icon;
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();

        Room room = Get<Room>();
        Icon.sprite = room.Descriptor.GetSprite().Sprite;
    }
}
