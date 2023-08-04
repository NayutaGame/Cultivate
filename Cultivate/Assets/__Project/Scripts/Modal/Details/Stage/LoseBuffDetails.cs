using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseBuffDetails : StageEventDetails
{
    public Buff _buff;

    public LoseBuffDetails(Buff buff)
    {
        _buff = buff;
    }
}
