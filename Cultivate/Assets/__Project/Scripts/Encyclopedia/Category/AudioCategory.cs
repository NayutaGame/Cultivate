
using System.Collections.Generic;

public class AudioCategory : Category<AudioEntry>
{
    public AudioCategory()
    {
        AddRange(new List<AudioEntry>()
        {
            // Rhythm
            new("Audio0001", "BGMBoss", "event:/BGM/BGMBoss", AudioEntry.AudioType.Music),
            new("Audio0002", "BGMElite1", "event:/BGM/BGMElite1", AudioEntry.AudioType.Music),
            new("Audio0003", "BGMElite2", "event:/BGM/BGMElite2", AudioEntry.AudioType.Music),
            new("Audio0004", "BGMTitle", "event:/BGM/BGMTitle", AudioEntry.AudioType.Music),
            new("Audio0005", "BGMLianQi", "event:/BGM/BGMLianQi", AudioEntry.AudioType.Music),
            new("Audio0006", "BGMZhuJi", "event:/BGM/BGMZhuJi", AudioEntry.AudioType.Music),
            new("Audio0007", "BGMJinDan", "event:/BGM/BGMJinDan", AudioEntry.AudioType.Music),
            new("Audio0008", "BGMYuanYing", "event:/BGM/BGMYuanYing", AudioEntry.AudioType.Music),
            new("Audio0009", "BGMHuaShen", "event:/BGM/BGMHuaShen", AudioEntry.AudioType.Music),

            // SFX
            new("Audio0010", "Backward", "event:/SFX/UI/Backward", AudioEntry.AudioType.SFX),
            new("Audio0011", "ButtonHover", "event:/SFX/UI/ButtonHover", AudioEntry.AudioType.SFX),
            new("Audio0012", "ButtonPress", "event:/SFX/UI/ButtonPress", AudioEntry.AudioType.SFX),
            new("Audio0013", "CardHover", "event:/SFX/UI/CardHover", AudioEntry.AudioType.SFX),
            new("Audio0014", "CardPlacement", "event:/SFX/UI/CardPlacement", AudioEntry.AudioType.SFX),
            new("Audio0015", "CardUpgrade", "event:/SFX/UI/CardUpgrade", AudioEntry.AudioType.SFX),
            new("Audio0016", "ComicPanelProceed", "event:/SFX/UI/ComicPanelProceed", AudioEntry.AudioType.SFX),
            new("Audio0017", "EnterAdventure", "event:/SFX/UI/EnterAdventure", AudioEntry.AudioType.SFX),
            new("Audio0018", "EnterSettings", "event:/SFX/UI/EnterSettings", AudioEntry.AudioType.SFX),
            new("Audio0019", "EnterShop", "event:/SFX/UI/EnterShop", AudioEntry.AudioType.SFX),
            new("Audio0020", "EnterStage", "event:/SFX/UI/EnterStage", AudioEntry.AudioType.SFX),
            new("Audio0021", "ExitSettings", "event:/SFX/UI/ExitSettings", AudioEntry.AudioType.SFX),
            new("Audio0022", "ExitStage", "event:/SFX/UI/ExitStage", AudioEntry.AudioType.SFX),
            new("Audio0023", "FinishGuide", "event:/SFX/UI/FinishGuide", AudioEntry.AudioType.SFX),
            new("Audio0024", "FormationHover", "event:/SFX/UI/FormationHover", AudioEntry.AudioType.SFX),
            new("Audio0025", "Forward", "event:/SFX/UI/Forward", AudioEntry.AudioType.SFX),
            new("Audio0026", "GainGold", "event:/SFX/UI/GainGold", AudioEntry.AudioType.SFX),
            new("Audio0027", "GainMaxHealth", "event:/SFX/UI/GainMaxHealth", AudioEntry.AudioType.SFX),
            new("Audio0028", "GainMingYuan", "event:/SFX/UI/GainMingYuan", AudioEntry.AudioType.SFX),
            new("Audio0029", "GainSkill", "event:/SFX/UI/GainSkill", AudioEntry.AudioType.SFX),
            new("Audio0030", "ItemHover", "event:/SFX/UI/ItemHover", AudioEntry.AudioType.SFX),
            new("Audio0031", "JingJieSwitch", "event:/SFX/UI/JingJieSwitch", AudioEntry.AudioType.SFX),
            new("Audio0032", "LoseGold", "event:/SFX/UI/LoseGold", AudioEntry.AudioType.SFX),
            new("Audio0033", "LoseMingYuan", "event:/SFX/UI/LoseMingYuan", AudioEntry.AudioType.SFX),
            new("Audio0034", "ShowDiscovered", "event:/SFX/UI/ShowDiscovered", AudioEntry.AudioType.SFX),
            new("Audio0035", "Sort", "event:/SFX/UI/Sort", AudioEntry.AudioType.SFX),

            // VFX
            new("Audio0036", "BuffVFX", "event:/SFX/VFX/BuffVFX", AudioEntry.AudioType.SFX),
            new("Audio0037", "DebuffVFX", "event:/SFX/VFX/DebuffVFX", AudioEntry.AudioType.SFX),
            new("Audio0038", "HealVFX", "event:/SFX/VFX/HealVFX", AudioEntry.AudioType.SFX),
            new("Audio0039", "HitVFXHuo", "event:/SFX/VFX/HitVFXHuo", AudioEntry.AudioType.SFX),
            new("Audio0040", "HitVFXJin", "event:/SFX/VFX/HitVFXJin", AudioEntry.AudioType.SFX),
            new("Audio0041", "HitVFXMu", "event:/SFX/VFX/HitVFXMu", AudioEntry.AudioType.SFX),
            new("Audio0042", "HitVFXShui", "event:/SFX/VFX/HitVFXShui", AudioEntry.AudioType.SFX),
            new("Audio0043", "HitVFXTu", "event:/SFX/VFX/HitVFXTu", AudioEntry.AudioType.SFX),
            new("Audio0044", "PiercingVFXHuo", "event:/SFX/VFX/PiercingVFXHuo", AudioEntry.AudioType.SFX),
            new("Audio0045", "PiercingVFXJin", "event:/SFX/VFX/PiercingVFXJin", AudioEntry.AudioType.SFX),
            new("Audio0046", "PiercingVFXMu", "event:/SFX/VFX/PiercingVFXMu", AudioEntry.AudioType.SFX),
            new("Audio0047", "PiercingVFXShui", "event:/SFX/VFX/PiercingVFXShui", AudioEntry.AudioType.SFX),
            new("Audio0048", "PiercingVFXTu", "event:/SFX/VFX/PiercingVFXTu", AudioEntry.AudioType.SFX),
            new("Audio0049", "GainArmor", "event:/SFX/VFX/GainArmor", AudioEntry.AudioType.SFX),
            new("Audio0050", "LoseArmor", "event:/SFX/VFX/LoseArmor", AudioEntry.AudioType.SFX),
            new("Audio0051", "Dodge", "event:/SFX/VFX/Dodge", AudioEntry.AudioType.SFX),
            new("Audio0052", "GainMana", "event:/SFX/VFX/GainMana", AudioEntry.AudioType.SFX),
            new("Audio0053", "FragileDamaged", "event:/SFX/VFX/FragileDamaged", AudioEntry.AudioType.SFX),
            new("Audio0054", "FullDefense", "event:/SFX/VFX/FullDefense", AudioEntry.AudioType.SFX),
            new("Audio0055", "Burn", "event:/SFX/VFX/Burn", AudioEntry.AudioType.SFX),
        });
    }
}
