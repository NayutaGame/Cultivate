
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RunConfig : Addressable, ISerializationCallbackReceiver
{
    [SerializeReference] public CharacterProfile CharacterProfile;
    [SerializeReference] public DifficultyProfile DifficultyProfile;
    [SerializeReference] public List<PackEntry> PacksToStartWith;
    [SerializeField] public MapEntry MapEntry;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public RunConfig(CharacterProfile characterProfile,
        DifficultyProfile difficultyProfile,
        List<PackEntry> packsToStartWith = null,
        MapEntry mapEntry = null)
    {
        _accessors = new()
        {
            { "CharacterProfile", () => CharacterProfile },
        };
        
        CharacterProfile = characterProfile;
        DifficultyProfile = difficultyProfile;
        PacksToStartWith = packsToStartWith ?? GetCharacter().GetDefaultPacks();
        // MapEntry = mapEntry ?? "标准无教程";
        MapEntry = mapEntry ?? "标准";
        // MapEntry = mapEntry ?? "发现";
        
        // MapEntry = mapEntry ?? "测试";
        // MapEntry = mapEntry ?? "墨染测试";
        // MapEntry = mapEntry ?? "动画测试";
        // MapEntry = mapEntry ?? "境界测试";
        // MapEntry = mapEntry ?? "多段测试";
        // MapEntry = mapEntry ?? "拖拽测试";
        // MapEntry = mapEntry ?? "结算测试";
        // MapEntry = mapEntry ?? "排局3";
        // MapEntry = mapEntry ?? "商店测试";
        // MapEntry = mapEntry ?? "教学10";
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "CharacterProfile", () => CharacterProfile },
        };
        
        MapEntry = string.IsNullOrEmpty(MapEntry.GetId()) ? null : Encyclopedia.MapCategory[MapEntry.GetId()];

        for (int i = 0; i < PacksToStartWith.Count; i++)
        {
            PackEntry entry = PacksToStartWith[i];
            PacksToStartWith[i] = string.IsNullOrEmpty(entry.GetId()) ? null : Encyclopedia.PackCategory[entry.GetId()];
        }
    }

    public static RunConfig FirstRun()
    {
        Profile profile = AppManager.Instance.ProfileManager.ProfileList[0];
        return new(profile.CharacterProfileList[0], profile.DifficultyProfileList[0], null, "序章");
    }

    public static RunConfig LastDifficulty()
    {
        Profile profile = AppManager.Instance.ProfileManager.ProfileList[0];
        return new(profile.CharacterProfileList[0], profile.DifficultyProfileList[10], null, "标准");
    }

    public CharacterEntry GetCharacter()
        => CharacterProfile.GetEntry();

    public int GetDifficulty()
        => DifficultyProfile.GetEntry()._order;
}
