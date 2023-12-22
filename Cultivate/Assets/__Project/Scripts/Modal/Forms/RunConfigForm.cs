using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunConfigForm
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public RunConfigForm(CharacterProfile characterProfile, DifficultyProfile difficultyProfile)
    {
        CharacterProfile = characterProfile;
        DifficultyProfile = difficultyProfile;
    }
}
