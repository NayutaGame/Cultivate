
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class DifficultyProfileList : ListModel<DifficultyProfile>, ISerializationCallbackReceiver
{
    private DifficultyProfileList()
    {
        Encyclopedia.DifficultyCategory.Do(entry => Add(new DifficultyProfile(entry)));

        Find("0").SetUnlocked(true);
    }

    public DifficultyProfile Find(DifficultyEntry entry)
        => First(difficultyProfile => difficultyProfile.GetEntry() == entry);

    public static DifficultyProfileList Default()
        => new();

    public void UnlockEverything()
    {
        this.Do(difficultyProfile => difficultyProfile.SetUnlocked(true));
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }

    public void UnlockDifficulty(DifficultyEntry difficultyEntry)
    {
        Find(difficultyEntry).SetUnlocked(true);
    }

    public DifficultyEntry GetCurrentHighestUnlockedDifficulty()
    {
        DifficultyEntry highestUnlocked = this[0].GetEntry();
        foreach (DifficultyProfile difficultyProfile in this)
        {
            if (!difficultyProfile.IsUnlocked())
                return highestUnlocked;
            highestUnlocked = difficultyProfile.GetEntry();
        }

        return highestUnlocked;
    }
}
