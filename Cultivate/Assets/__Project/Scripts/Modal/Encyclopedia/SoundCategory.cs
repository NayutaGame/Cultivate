using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCategory : Category<SoundEntry>
{
    public SoundCategory()
    {
        List = new()
        {
            new("奇遇", "Images/NodeIcons/Adventure", SoundEntry.AudioType.Music),
        };
    }
}
