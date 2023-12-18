
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class DifficultyProfileList : ListModel<DifficultyProfile>, ISerializationCallbackReceiver
{
    private DifficultyProfileList()
    {
        Encyclopedia.DifficultyCategory.Traversal.Do(entry => Add(new DifficultyProfile(entry, true)));

        // Encyclopedia.DifficultyCategory.Traversal.Do(entry => Add(new DifficultyProfile(entry)));
        // Find("0").SetUnlocked(true);
        // Find("-1").SetUnlocked(true);
    }

    private DifficultyProfile Find(DifficultyEntry entry)
        => First(difficultyProfile => difficultyProfile.GetEntry() == entry);

    public static DifficultyProfileList Default()
        => new();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }
}
