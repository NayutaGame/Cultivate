
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RoomPool : FinitePool<RoomEntry>, ISerializationCallbackReceiver
{
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        int count = Count();
        RoomEntry[] tempRoomList = new RoomEntry[count];

        for (int i = 0; i < tempRoomList.Length; i++)
        {
            TryPopItem(out RoomEntry popped);
            tempRoomList[i] = string.IsNullOrEmpty(popped.GetId()) ? null : Encyclopedia.RoomCategory.FromId(popped.GetId());
        }

        Populate(tempRoomList);
    }
}
