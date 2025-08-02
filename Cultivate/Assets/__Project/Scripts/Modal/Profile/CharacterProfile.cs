
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterProfile : ISerializationCallbackReceiver, AnnotatableCharacter
{
    private const int PACK_SLOT_COUNT = 7;

    [SerializeField] private CharacterEntry _entry;
    [SerializeField] public int _level;
    [SerializeField] public int _experience;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public CharacterProfile(CharacterEntry entry)
    {
        _entry = entry;
        _level = 1;
        _experience = 0;
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

    public bool IsDemoLocked()
        => _entry != Encyclopedia.CharacterCategory.FromName("徐福") && AppManager.Instance.PackageIsDemo();

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
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.CharacterCategory.FromId(_entry.GetId());
    }

    public bool CanShowAnnotation()
        => true;

    public string GetTitle()
        => GetEntry().GetName();

    public Description GetAbilityDescription()
        => GetEntry().GetAbilityDescription();
}
