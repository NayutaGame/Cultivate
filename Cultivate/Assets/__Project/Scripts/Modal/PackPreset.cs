
using System.Collections.Generic;

public class PackPreset
{
    public List<PackEntry> PackEntries;

    public PackPreset(List<PackEntry> packEntries)
    {
        PackEntries = packEntries;
    }

    public static PackPreset Default => new(new List<PackEntry> {
        Encyclopedia.PackCategory["0001"],
        Encyclopedia.PackCategory["0003"],
        Encyclopedia.PackCategory["0005"],
        Encyclopedia.PackCategory["0007"],
        Encyclopedia.PackCategory["0009"],
        Encyclopedia.PackCategory["0008"],
        Encyclopedia.PackCategory["0010"],
    });
}
