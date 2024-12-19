
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillPool : Pool<SkillEntry>, ISerializationCallbackReceiver
{
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        int count = List.Count;
        SkillEntry[] tempRoomList = new SkillEntry[count];

        for (int i = 0; i < tempRoomList.Length; i++)
        {
            TryPopItem(out SkillEntry popped);
            tempRoomList[i] = string.IsNullOrEmpty(popped.GetName()) ? null : Encyclopedia.SkillCategory[popped.GetName()];
        }

        Populate(tempRoomList);
    }
}
