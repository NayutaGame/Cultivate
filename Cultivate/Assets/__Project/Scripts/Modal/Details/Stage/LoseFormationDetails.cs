using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseFormationDetails : EventDetails
{
    public Formation _formation;

    public LoseFormationDetails(Formation formation)
    {
        _formation = formation;
    }
}
