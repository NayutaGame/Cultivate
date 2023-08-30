using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationDetails : EventDetails
{
    public StageEntity Owner;
    public FormationEntry _formation;
    public bool _recursive;

    public FormationDetails(StageEntity owner, FormationEntry formation, bool recursive = true)
    {
        Owner = owner;
        _formation = formation;
        _recursive = recursive;
    }
}
