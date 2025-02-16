
using System;
using UnityEngine;

[Serializable]
public class PackProfile : ISerializationCallbackReceiver
{
    [SerializeField] private PackEntry _entry;

    public PackProfile(PackEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
    }
    
    public PackEntry GetEntry() => _entry;

    public bool IsUnlocked()
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        var achievementProfile = profile.GetAchievementProfileFromLockIndex(ToLockIndex());
        if (achievementProfile == null)
            return true;
        return achievementProfile.IsUnlocked();
    }

    public void SetUnlockedQuietly(bool value)
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        var achievementProfile = profile.GetAchievementProfileFromLockIndex(ToLockIndex());
        if (achievementProfile == null)
            return;
        achievementProfile.SetUnlockedQuietly(value);
    }

    public LockIndex ToLockIndex() => LockIndex.FromPack(_entry.GetId());

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.PackCategory[_entry.GetId()];
    }
}
