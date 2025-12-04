using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromBarterToHandDetails : RunClosureDetails
{
    public RunSkill FromSkill;
    public DeckIndex FromIndex;

    public FromBarterToHandDetails(RunSkill fromSkill, DeckIndex fromIndex)
    {
        FromSkill = fromSkill;
        FromIndex = fromIndex;
    }
}