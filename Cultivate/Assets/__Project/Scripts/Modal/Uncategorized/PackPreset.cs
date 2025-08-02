
using System.Collections.Generic;

public class PackPreset
{
    public List<PackEntry> PackEntries;

    public PackPreset(List<PackEntry> packEntries)
    {
        PackEntries = packEntries;
    }

    public static PackPreset Default => new(new List<PackEntry> {
        Encyclopedia.PackCategory.FromId("0001"),
        Encyclopedia.PackCategory.FromId("0003"),
        Encyclopedia.PackCategory.FromId("0005"),
        Encyclopedia.PackCategory.FromId("0007"),
        Encyclopedia.PackCategory.FromId("0009"),
        Encyclopedia.PackCategory.FromId("0011"),
        Encyclopedia.PackCategory.FromId("0012"),
    });
}
