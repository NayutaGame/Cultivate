using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCategory : Category<AudioEntry>
{
    public AudioCategory()
    {
        List = new()
        {
            new("练气BGM", "Audios/Music/Welcome to Ming Dynasty", AudioEntry.AudioType.Music),
            new("筑基BGM", "Audios/Music/Oriental Dubstep", AudioEntry.AudioType.Music),
            new("金丹BGM", "Audios/Music/Explore Modern China", AudioEntry.AudioType.Music),
            new("元婴BGM", "Audios/Music/Korean Chase", AudioEntry.AudioType.Music),
            new("化神BGM", "Audios/Music/Japanese Car Chase", AudioEntry.AudioType.Music),
            new("钱币", "Audios/SFX/Coins", AudioEntry.AudioType.SFX),
        };
    }
}
