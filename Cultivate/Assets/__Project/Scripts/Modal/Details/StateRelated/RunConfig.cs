
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

        if (!AppManager.Instance.AudienceIsDeveloper())
        {
            MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("标准");
        }
        else
        {
            MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("标准");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("返虚测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("墨染测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("标准无教程");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("发现");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("动画测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("境界测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("多段测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("拖拽测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("结算测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("商店测试");
            // MapEntry = mapEntry ?? Encyclopedia.MapCategory.FromName("教学10");
        }
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        MapEntry = string.IsNullOrEmpty(MapEntry.GetId()) ? null : Encyclopedia.MapCategory.FromId(MapEntry.GetId());

        for (int i = 0; i < PacksToStartWith.Count; i++)
        {
            PackEntry entry = PacksToStartWith[i];
            PacksToStartWith[i] = string.IsNullOrEmpty(entry.GetId()) ? null : Encyclopedia.PackCategory.FromId(entry.GetId());
        }
    }

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
