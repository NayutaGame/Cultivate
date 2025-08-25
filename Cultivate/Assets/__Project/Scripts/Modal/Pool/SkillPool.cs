
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillPool : FinitePool<SkillEntry>, ISerializationCallbackReceiver
{
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        int count = Count();
        SkillEntry[] tempRoomList = new SkillEntry[count];

        for (int i = 0; i < tempRoomList.Length; i++)
        {
            TryPopItem(out SkillEntry popped);
            tempRoomList[i] = string.IsNullOrEmpty(popped.GetId()) ? null : Encyclopedia.SkillCategory.FromId(popped.GetId());
        }

        Populate(tempRoomList);
    }
}
