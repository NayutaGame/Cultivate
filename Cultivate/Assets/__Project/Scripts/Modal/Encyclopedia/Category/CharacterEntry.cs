
using System;
using UnityEngine;

public class CharacterEntry : Entry
{
    public string GetName() => GetId();
    
    public string Description;
    public string AbilityDescription;

    public RunClosure[] _runClosures;
    public StageClosure[] _stageClosures;

    private SpriteEntry _spriteEntry;

    private PrefabEntry _stageModel;

    public CharacterEntry(string id, string description = null, string abilityDescription = null,
        RunClosure[] runClosures = null,
        StageClosure[] stageClosures = null) : base(id)
    {
        Description = description ?? "没有描述";
        AbilityDescription = abilityDescription ?? "没有技能描述";

        _runClosures = runClosures ?? Array.Empty<RunClosure>();
        _stageClosures = stageClosures ?? Array.Empty<StageClosure>();

        _spriteEntry = GetName();
        _stageModel = GetName();
    }

    public static implicit operator CharacterEntry(string id) => Encyclopedia.CharacterCategory[id];
    
    // public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    public PrefabEntry GetStagePrefabEntry() => _stageModel ?? Encyclopedia.PrefabCategory.MissingStageModel();
}
