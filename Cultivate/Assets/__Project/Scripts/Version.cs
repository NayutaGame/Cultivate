
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

    public bool IsProfileCompatible(Version other)
    {
        return Major == other.Major && Minor >= other.Minor;
    }
    
    public bool IsRunCompatible(Version other)
    {
        return Major == other.Major &&
               Minor == other.Minor &&
               Patch >= other.Patch;
    }
    
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}