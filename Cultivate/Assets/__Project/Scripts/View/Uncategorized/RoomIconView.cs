
using UnityEngine;
using UnityEngine.UI;

public class RoomIconView : LegacySimpleView
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

        switch (room.State)
        {
            case Room.RoomState.Past:
                Icon.color = new Color(1, 1, 1, 0.4f);
                Icon.transform.localScale = Vector3.one;
                break;
            case Room.RoomState.Curr:
                Icon.color = new Color(1, 1, 1, 1);
                Icon.transform.localScale = Vector3.one * 1.6f;
                break;
            case Room.RoomState.Future:
                Icon.color = new Color(1, 1, 1, 1);
                Icon.transform.localScale = Vector3.one;
                break;
        }
    }
}
