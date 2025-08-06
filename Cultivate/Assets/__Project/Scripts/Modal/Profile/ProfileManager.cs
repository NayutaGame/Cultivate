
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
        ValidateProfileList();
    }

    private void ValidateProfileList()
    {
        bool profileExists = FileUtility.IsPersistentFileExists(ProfileList.Filename);
        if (profileExists)
            LoadProcedure();
        
        bool profileIsValid = _profileList != null && _profileList.IsCompatible();
        if (profileIsValid)
        {
            _profileList.Migrate();
            return;
        }

        CreateNewProfile();
    }

    private void LoadProcedure()
    {
        try
        {
            _profileList = FileUtility.ReadPersistentFile<ProfileList>(ProfileList.Filename);
        }
        catch
        {
            _profileList = null;
        }
    }

    public void SaveProcedure()
    {
        FileUtility.WritePersistentFile(_profileList, ProfileList.Filename);
    }

    private void CreateNewProfile()
    {
        _profileList = ProfileList.Default();
        GetCurrProfile().Environment = null;
        SaveProcedure();
    }

    public void DeleteProfile()
    {
        CreateNewProfile();
    }

    public Profile GetCurrProfile()
        => _profileList.GetCurrent();
}
