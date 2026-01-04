
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RunConfig : Addressable, ISerializationCallbackReceiver
{
    [SerializeReference] public CharacterProfile CharacterProfile;
    [SerializeReference] public DifficultyProfile DifficultyProfile;
    [SerializeReference] public List<PackEntry> PacksToStartWith;

    [NonSerialized] private MergeRule[] _defaultMergeRules;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "CharacterProfile",           thisObject => ((RunConfig)thisObject).CharacterProfile },
    };
    public object Get(string s) => Accessor[s](this);
    public RunConfig(CharacterProfile characterProfile,
        DifficultyProfile difficultyProfile,
        List<PackEntry> packsToStartWith = null,
        MapEntry mapEntry = null)
    {
        CharacterProfile = characterProfile;
        DifficultyProfile = difficultyProfile;
        PacksToStartWith = packsToStartWith ?? GetCharacter().GetDefaultPacks();

        _defaultMergeRules = DifficultyProfile.GetEntry().AllowFanXuMerge
            ? MergeRule.DefaultMergeRules
            : MergeRule.DefaultMergeRulesLockFanXu;
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < PacksToStartWith.Count; i++)
        {
            PackEntry entry = PacksToStartWith[i];
            PacksToStartWith[i] = string.IsNullOrEmpty(entry.GetId()) ? null : Encyclopedia.PackCategory.FromId(entry.GetId());
        }

        _defaultMergeRules = DifficultyProfile.GetEntry().AllowFanXuMerge
            ? MergeRule.DefaultMergeRules
            : MergeRule.DefaultMergeRulesLockFanXu;
    }

    public MergeRule[] GetDefaultMergeRules()
        => _defaultMergeRules;

    public static RunConfig FirstRun()
    {
        Profile profile = AppManager.Instance.ProfileManager.ProfileList[0];
        return new(profile.CharacterProfileList[0], profile.DifficultyProfileList[0], null, Encyclopedia.MapCategory.FromName("序章"));
    }

    public static RunConfig LastDifficulty()
    {
        Profile profile = AppManager.Instance.ProfileManager.ProfileList[0];
        return new(profile.CharacterProfileList[0], profile.DifficultyProfileList[10], null, Encyclopedia.MapCategory.FromName("标准"));
    }

    public CharacterEntry GetCharacter()
        => CharacterProfile.GetEntry();

    public int GetDifficulty()
        => DifficultyProfile.GetEntry()._order;
}
