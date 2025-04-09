
using System;
using UnityEngine;

[Serializable]
public class CharacterProfile : ISerializationCallbackReceiver
{
    private const int PACK_SLOT_COUNT = 7;

    [SerializeField] private CharacterEntry _entry;
    [SerializeField] private int _level;
    [SerializeField] private int _experience;

    public CharacterProfile(CharacterEntry entry, bool isDeveloper = false)
    {
        _entry = entry;
        _level = isDeveloper ? 10 : 1;
        _experience = isDeveloper ? 1000 : 0;
    }
    
    public CharacterEntry GetEntry() => _entry;

    public LockIndex ToCharacterLockIndex() => LockIndex.FromCharacter(_entry);
    public LockIndex ToSlotLockIndex(int slotIndex) => LockIndex.FromSlot(_entry, slotIndex);

    public bool IsUnlocked()
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        var achievementProfile = profile.GetAchievementProfileFromLockIndex(ToCharacterLockIndex());
        if (achievementProfile == null)
            return true;
        return achievementProfile.IsUnlocked();
    }

    public bool SlotIsUnlocked(int slotIndex)
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        var achievementProfile = profile.GetAchievementProfileFromLockIndex(ToSlotLockIndex(slotIndex));
        if (achievementProfile == null)
            return true;
        return achievementProfile.IsUnlocked();
    }

    public void SetUnlockedQuietly(bool value)
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        var achievementProfile = profile.GetAchievementProfileFromLockIndex(ToCharacterLockIndex());
        if (achievementProfile == null)
            return;
        achievementProfile.SetUnlockedQuietly(value);
    }

    public void SetSlotUnlockedQuietly(int slotIndex, bool value)
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        var achievementProfile = profile.GetAchievementProfileFromLockIndex(ToSlotLockIndex(slotIndex));
        if (achievementProfile == null)
            return;
        achievementProfile.SetUnlockedQuietly(value);
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.CharacterCategory[_entry.GetId()];
    }
}
