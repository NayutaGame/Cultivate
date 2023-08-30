using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainFormationDetails : EventDetails
{
    public FormationEntry _entry;

    public GainFormationDetails(FormationEntry entry)
    {
        _entry = entry;
    }
}
