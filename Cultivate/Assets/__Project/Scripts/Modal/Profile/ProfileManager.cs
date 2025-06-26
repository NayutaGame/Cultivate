
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

        LoadOrDefault();
    }

    private void LoadOrDefault()
    {
        if (!FileUtility.IsPersistentFileExists(ProfileList.Filename))
        {
            NewProfileProcedure();
        }
        else
        {
            LoadProcedure();
        }
    }

    public void NewProfileProcedure()
    {
        _profileList = ProfileList.Default();
        SaveProcedure();
    }

    public void UnlockEverythingProcedure()
    {
        GetCurrProfile().UnlockEverything();
        SaveProcedure();
    }

    public void SaveProcedure()
    {
        FileUtility.WritePersistentFile(_profileList, ProfileList.Filename);
    }

    public void SaveProcedureRemovingRunEnvironment()
    {
        GetCurrProfile().RunEnvironment = null;
        FileUtility.WritePersistentFile(_profileList, ProfileList.Filename);
    }

    public void SaveProcedure(RunEnvironment env)
    {
        env.WriteTime();
        GetCurrProfile().WriteRunEnvironment(env);
        FileUtility.WritePersistentFile(_profileList, ProfileList.Filename);
    }

    public void SaveProcedureForAchievements(AchievementEntry entry)
    {
        ProfileList lastSavedProfileList = FileUtility.ReadPersistentFile<ProfileList>(ProfileList.Filename);
        Profile profile = lastSavedProfileList.GetCurrent();
        
        var achievementProfile = profile.AchievementProfileList.First(ap => ap.GetEntry().GetId() == entry.GetId());
            
        if (achievementProfile == null)
        {
            Debug.LogError($"未找到成就: {entry.GetId()}");
            return;
        }
        
        achievementProfile.SetUnlockedQuietly(true);
        FileUtility.WritePersistentFile(lastSavedProfileList, ProfileList.Filename);
    }

    public void LoadProcedure()
    {
        _profileList = FileUtility.ReadPersistentFile<ProfileList>(ProfileList.Filename);
        // case存档损坏
    }

    public void WriteRunResultToCurrent(RunEnvironment env, RunResult result, int experienceGain)
    {
        GetCurrProfile().WriteRunResult(env, result, experienceGain);
        SaveProcedureRemovingRunEnvironment();
    }

    public Profile GetCurrProfile()
        => _profileList.GetCurrent();
}
