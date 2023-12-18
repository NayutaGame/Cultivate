
using System;

[Serializable]
public class DifficultyProfile
{
    private DifficultyEntry _entry;
    public DifficultyEntry GetEntry() => _entry;

    private bool _unlocked;
    public bool IsUnlocked() => _unlocked;
    public void SetUnlocked(bool value) => _unlocked = value;

    public DifficultyProfile(DifficultyEntry entry, bool unlocked = false)
    {
        _entry = entry;
        _unlocked = unlocked;
    }
}
