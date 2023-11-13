
using System.Collections.Generic;

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

            new("CardPlacement", "event:/SFX/UI/CardPlacement", AudioEntry.AudioType.SFX),
            new("CardHover", "event:/SFX/UI/CardHover", AudioEntry.AudioType.SFX),
            new("CardUpgrade", "event:/SFX/UI/CardUpgrade", AudioEntry.AudioType.SFX),
            new("Forward", "event:/SFX/UI/Forward", AudioEntry.AudioType.SFX),
            new("Backward", "event:/SFX/UI/Backward", AudioEntry.AudioType.SFX),

            new("BuffVFX", "event:/SFX/VFX/BuffVFX", AudioEntry.AudioType.SFX),
            new("DebuffVFX", "event:/SFX/VFX/DebuffVFX", AudioEntry.AudioType.SFX),
            new("HealVFX", "event:/SFX/VFX/HealVFX", AudioEntry.AudioType.SFX),
            new("HitVFXHuo", "event:/SFX/VFX/HitVFXHuo", AudioEntry.AudioType.SFX),
            new("HitVFXJin", "event:/SFX/VFX/HitVFXJin", AudioEntry.AudioType.SFX),
            new("HitVFXMu", "event:/SFX/VFX/HitVFXMu", AudioEntry.AudioType.SFX),
            new("HitVFXShui", "event:/SFX/VFX/HitVFXShui", AudioEntry.AudioType.SFX),
            new("HitVFXTu", "event:/SFX/VFX/HitVFXTu", AudioEntry.AudioType.SFX),
            new("PiercingVFXHuo", "event:/SFX/VFX/PiercingVFXHuo", AudioEntry.AudioType.SFX),
            new("PiercingVFXJin", "event:/SFX/VFX/PiercingVFXJin", AudioEntry.AudioType.SFX),
            new("PiercingVFXMu", "event:/SFX/VFX/PiercingVFXMu", AudioEntry.AudioType.SFX),
            new("PiercingVFXShui", "event:/SFX/VFX/PiercingVFXShui", AudioEntry.AudioType.SFX),
            new("PiercingVFXTu", "event:/SFX/VFX/PiercingVFXTu", AudioEntry.AudioType.SFX),
        });
    }
}
