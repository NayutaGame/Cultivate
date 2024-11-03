
using System;
using System.Collections.Generic;
using CLLibrary;

public class ProfileManager : Addressable
{
    private ProfileList _profileList;
    public ProfileList ProfileList => _profileList;

    public RunConfigForm RunConfigForm;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileManager()
    {
        _accessors = new()
        {
            { "ProfileList",           () => _profileList },
        };

        Load();
    }

    public void Save()
    {
        FileUtility.WriteToFile(_profileList, ProfileList.Filename);
    }

    public void Load()
    {
        _profileList = FileUtility.ReadFromFile<ProfileList>(ProfileList.Filename);
    }

    public static void WriteRunResultToCurrent(RunResult result)
        => AppManager.Instance.ProfileManager._profileList.GetCurrent().WriteRunResult(result);
}
