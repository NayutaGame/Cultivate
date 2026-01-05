
using UnityEngine;
using UnityEngine.UI;

public class RoomIconView : XView
{
    [SerializeField] private Image Icon;

    public override void Refresh()
    {
        base.Refresh();

        LegacyRoom room = Get<LegacyRoom>();
        Icon.sprite = room.GetDescriptor().GetSprite().Sprite;

        switch (room.GetState())
        {
            case LegacyRoom.RoomState.Past:
                Icon.color = new Color(1, 1, 1, 0.4f);
                Icon.transform.localScale = Vector3.one;
                break;
            case LegacyRoom.RoomState.Curr:
                Icon.color = new Color(1, 1, 1, 1);
                Icon.transform.localScale = Vector3.one * 1.6f;
                break;
            case LegacyRoom.RoomState.Future:
                Icon.color = new Color(1, 1, 1, 1);
                Icon.transform.localScale = Vector3.one;
                break;
        }
    }
}
