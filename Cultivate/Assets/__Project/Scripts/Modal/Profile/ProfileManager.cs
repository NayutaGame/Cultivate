
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ProfileManager : Addressable
{
    private ProfileList _profileList;
    public ProfileList ProfileList => _profileList;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "ProfileList",                thisObject => ((ProfileManager)thisObject)._profileList },
        { "Curr",                       thisObject => ((ProfileManager)thisObject).GetCurrProfile() },
    };
    public object Get(string s) => Accessor[s](this);
    public ProfileManager()
    {
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

    public void SaveProcedure()
    {
        FileUtility.WritePersistentFile(_profileList, ProfileList.Filename);
    }
    public void LoadProcedure()
    {
        _profileList = FileUtility.ReadPersistentFile<ProfileList>(ProfileList.Filename);
        // case存档损坏
    }

    public Profile GetCurrProfile()
        => _profileList.GetCurrent();
}
