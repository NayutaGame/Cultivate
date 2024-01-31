
using System;
using System.Collections.Generic;
using CLLibrary;

public class ProfileManager : Addressable
{
    private ProfileList ProfileList;

    public RunConfigForm RunConfigForm;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public ProfileManager()
    {
        _accessors = new()
        {
            { "ProfileList",           () => ProfileList },
        };

        Load();
    }

    public void Save()
    {
        FileUtility.WriteToFile(ProfileList, ProfileList.Filename);
    }

    public void Load()
    {
        ProfileList = FileUtility.ReadFromFile<ProfileList>(ProfileList.Filename);
    }

    public static void WriteRunResultToCurrent(RunResult result)
        => AppManager.Instance.ProfileManager.ProfileList.GetCurrent().WriteRunResult(result);
}
