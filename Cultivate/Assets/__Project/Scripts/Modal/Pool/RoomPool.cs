
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RoomPool : Pool<RoomEntry>, ISerializationCallbackReceiver
{
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        int count = List.Count;
        RoomEntry[] tempRoomList = new RoomEntry[count];

        for (int i = 0; i < tempRoomList.Length; i++)
        {
            TryPopItem(out RoomEntry popped);
            tempRoomList[i] = string.IsNullOrEmpty(popped.GetName()) ? null : Encyclopedia.RoomCategory[popped.GetName()];
        }

        Populate(tempRoomList);
    }
}
