
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class AchievementProfileList : ListModel<AchievementProfile>, ISerializationCallbackReceiver
{
    private AchievementProfileList()
    {
        Encyclopedia.AchievementCategory.Traversal.Do(entry => Add(new AchievementProfile(entry)));
    }

    private AchievementProfile Find(AchievementEntry entry)
        => First(achievementProfile => achievementProfile.GetEntry() == entry);

    public static AchievementProfileList Default()
        => new();

    public void UnlockEverything()
    {
        Traversal().Do(achievementProfile => achievementProfile.SetUnlockedQuietly(true));
    }

    public bool IsUnlocked(AchievementEntry entry)
        => Find(entry).IsUnlocked();
        
    // public int GetProgress(AchievementEntry entry)
    //     => Find(entry).GetProgress();
        
    // public void SetProgress(AchievementEntry entry, int progress)
    //     => Find(entry).SetProgress(progress);
        
    // public void AddProgress(AchievementEntry entry, int progress = 1)
    //     => Find(entry).AddProgress(progress);

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize() { }
}
