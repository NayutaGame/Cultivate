
using System;
using System.Collections.Generic;
using CLLibrary;

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

        Load();
    }

    public void Save()
    {
        AppManager.Instance.ProfileManager.GetCurrProfile().RunEnvironment = RunManager.Instance.Environment;
        RunManager.Instance.Environment.SaveProcedure();
        FileUtility.WriteToFile(_profileList, ProfileList.Filename);
    }

    public void Load()
    {
        _profileList = FileUtility.ReadFromFile<ProfileList>(ProfileList.Filename);
        // case存档损坏
    }

    public static void WriteRunResultToCurrent(RunResult result)
        => AppManager.Instance.ProfileManager._profileList.GetCurrent().WriteRunResult(result);

    public Profile GetCurrProfile()
        => _profileList.GetCurrent();
}
