
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementProfile : ISerializationCallbackReceiver, RunClosureListener, StageClosureListener, AnnotatableAchievement
{
    [SerializeField] private AchievementEntry _entry;
    [SerializeField] private bool _unlocked;

    [SerializeField] private SerializableDictionary _memory;

    public SerializableDictionary Memory => _memory;

    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public AchievementProfile(AchievementEntry entry)
    {
        _entry = entry;
        _unlocked = false;

        _memory = new();
    }
    
    public AchievementEntry GetEntry() => _entry;
    public bool IsUnlocked() => _unlocked;
    public void Unlock()
    {
        if (_unlocked) return;
        _unlocked = true;

        RunEnvironment env = RunManager.Instance.Environment;
        if (env != null)
            env.RecordNewlyUnlockedAchievement(_entry);
    }

    public void SetUnlockedQuietly(bool value)
    {
        _unlocked = value;
    }
    
    public void Reset()
    {
        _unlocked = false;
        _memory.Clear();
    }

    // public int GetProgress() => _progress;
    // public void SetProgress(int value) => _progress = value;
    // public void AddProgress(int value = 1) => _progress += value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.AchievementCategory.FromId(_entry.GetId());
    }

    public override string ToString()
    {
        return $"{_entry.GetId()}: {_unlocked}";
    }

    public bool CanShowAnnotation()
        => true;

    public string GetName()
        => _entry.GetName();

    public Description GetConditionDescription()
        => _entry.GetConditionDescription();

    public Description GetRewardDescription()
        => _entry.GetRewardDescription();
}
