
using System;

[Serializable]
public class CharacterProfile
{
    private CharacterEntry _entry;
    public CharacterEntry GetEntry() => _entry;

    private bool _unlocked;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;

    public CharacterProfile(CharacterEntry entry, bool unlocked = false)
    {
        _entry = entry;
        _unlocked = unlocked;
    }
}
