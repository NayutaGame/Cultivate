using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunConfig : Config
{
    public CharacterProfile CharacterProfile;
    public DifficultyProfile DifficultyProfile;

    public RunInitialCondition RunInitialCondition;

    public RunConfig(RunConfigForm runConfigForm, RunInitialCondition runInitialCondition)
    {
        CharacterProfile = runConfigForm.CharacterProfile;
        DifficultyProfile = runConfigForm.DifficultyProfile;

        RunInitialCondition = runInitialCondition;
    }
}
