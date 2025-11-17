
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RoomPool : FinitePool<LegacyRoomEntry>, ISerializationCallbackReceiver
{
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        int count = Count();
        LegacyRoomEntry[] tempRoomList = new LegacyRoomEntry[count];

        for (int i = 0; i < tempRoomList.Length; i++)
        {
            TryPopItem(out LegacyRoomEntry popped);
            tempRoomList[i] = string.IsNullOrEmpty(popped.GetId()) ? null : Encyclopedia.LegacyRoomCategory.FromId(popped.GetId());
        }

        Populate(tempRoomList);
    }
}
