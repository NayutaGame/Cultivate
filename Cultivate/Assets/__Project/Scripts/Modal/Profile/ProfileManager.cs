
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ProfileManager : Addressable
{
    private ProfileList _profileList;
    public ProfileList ProfileList => _profileList;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileManager()
    {
        _accessors = new()
        {
            { "ProfileList",           () => _profileList },
        };

        if (!FileUtility.IsFileExists(ProfileList.Filename))
        {
            // 如果存档不存在，则创建一个默认存档，并保存
            _profileList = ProfileList.Default();
            SaveProcedure();
        }
        else
        {
            // 如果存档存在，则加载存档
            LoadProcedure();
        }
    }

    public void SaveProcedure()
    {
        FileUtility.WriteToFile(_profileList, ProfileList.Filename);
    }

    public void SaveProcedure(RunEnvironment env)
    {
        env.WriteTime();
        GetCurrProfile().WriteRunEnvironment(env);
        FileUtility.WriteToFile(_profileList, ProfileList.Filename);
    }

    public void SaveProcedureForAchievements(AchievementEntry entry)
    {
        ProfileList lastSavedProfileList = FileUtility.ReadFromFile<ProfileList>(ProfileList.Filename);
        Profile profile = lastSavedProfileList.GetCurrent();
        
        var achievementProfile = profile.AchievementProfileList.First(ap => ap.GetEntry().GetId() == entry.GetId());
            
        if (achievementProfile == null)
        {
            Debug.LogError($"未找到成就: {entry.GetId()}");
            return;
        }
        
        achievementProfile.SetUnlockedQuietly(true);
        FileUtility.WriteToFile(lastSavedProfileList, ProfileList.Filename);
    }

    public void LoadProcedure()
    {
        _profileList = FileUtility.ReadFromFile<ProfileList>(ProfileList.Filename);
        // case存档损坏
    }

    public static void WriteRunResultToCurrent(RunResult result)
        => AppManager.Instance.ProfileManager._profileList.GetCurrent().WriteRunResult(result);

    public Profile GetCurrProfile()
        => _profileList.GetCurrent();
}
