
using System;

[Serializable]
public struct Version
{
    public int Major;
    public int Minor; 
    public int Patch;

    public Version(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public static bool IsProfileCompatible(Version v)
    {
        return AppManager.Instance._version.Major == v.Major;
    }
    
    public static bool IsRunCompatible(Version v)
    {
        return AppManager.Instance._version.Major == v.Major &&
               AppManager.Instance._version.Minor == v.Minor;
    }
    
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}