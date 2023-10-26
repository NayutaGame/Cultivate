using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCategory : Category<AudioEntry>
{
    public AudioCategory()
    {
        AddRange(new List<AudioEntry>()
        {
            // new("钱币", "Audios/SFX/Coins", AudioEntry.AudioType.SFX),

            new("BGMBoss", "event:/BGM/BGMBoss", AudioEntry.AudioType.Music),
            new("BGMElite1", "event:/BGM/BGMElite1", AudioEntry.AudioType.Music),
            new("BGMElite2", "event:/BGM/BGMElite2", AudioEntry.AudioType.Music),
            new("BGMTitle", "event:/BGM/BGMTitle", AudioEntry.AudioType.Music),
            new("BGMLianQi", "event:/BGM/BGMLianQi", AudioEntry.AudioType.Music),
            new("BGMZhuJi", "event:/BGM/BGMZhuJi", AudioEntry.AudioType.Music),
            new("BGMJinDan", "event:/BGM/BGMJinDan", AudioEntry.AudioType.Music),
            new("BGMYuanYing", "event:/BGM/BGMYuanYing", AudioEntry.AudioType.Music),
            new("BGMHuaShen", "event:/BGM/BGMHuaShen", AudioEntry.AudioType.Music),

            new("CardPlacement", "event:/SFX/CardPlacement", AudioEntry.AudioType.SFX),
            new("CardHover", "event:/SFX/CardHover", AudioEntry.AudioType.SFX),
            new("CardUpgrade", "event:/SFX/CardUpgrade", AudioEntry.AudioType.SFX),
            new("Forward", "event:/SFX/Forward", AudioEntry.AudioType.SFX),
            new("Backward", "event:/SFX/Backward", AudioEntry.AudioType.SFX),
        });
    }
}
