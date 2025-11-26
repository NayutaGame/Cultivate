
using TMPro;
using UnityEngine;

public class RoomBarView : XView
{
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        RoomEntry roomEntry = Get<RoomEntry>();
        NameText.text = roomEntry.GetName();
    }
}