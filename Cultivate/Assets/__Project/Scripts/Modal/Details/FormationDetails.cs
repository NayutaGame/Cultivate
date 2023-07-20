using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationDetails
{
    public StageEntity Owner;
    public FormationEntry _formation;
    public bool _recursive;
    public bool Cancel;

    public FormationDetails(StageEntity owner, FormationEntry formation, bool recursive = true, bool cancel = false)
    {
        Owner = owner;
        _formation = formation;
        _recursive = recursive;
        Cancel = cancel;
    }
}
