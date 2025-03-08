
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class DifficultyProfileList : ListModel<DifficultyProfile>, ISerializationCallbackReceiver
{
    private DifficultyProfileList(bool isDeveloper = false)
    {
        Encyclopedia.DifficultyCategory.Traversal.Do(entry => Add(new DifficultyProfile(entry, isDeveloper)));

        Find("0").SetUnlocked(true);
    }

    private DifficultyProfile Find(DifficultyEntry entry)
        => First(difficultyProfile => difficultyProfile.GetEntry() == entry);

    public static DifficultyProfileList Default()
        => new();

    public static DifficultyProfileList Developer()
        => new(true);

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // when new entry is added, order will be corrupted
        // needs to fix order according to encyclopedia before using
    }

    public void TryUnlockNextDifficulty(RunEnvironment env, RunResult result)
    {
        if (result.GetOutcome() != RunResult.RunOutcome.Victorious)
            return;

        DifficultyEntry curr = env.GetRunConfig().DifficultyProfile.GetEntry();
        DifficultyEntry next = Encyclopedia.DifficultyCategory.GetNext(curr);

        if (next == null)
            return;

        Find(next).SetUnlocked(true);
    }
}
