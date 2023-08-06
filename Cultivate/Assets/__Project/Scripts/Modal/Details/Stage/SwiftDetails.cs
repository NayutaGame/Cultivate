using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftDetails : StageEventDetails
{
    public StageEntity Owner;
    public bool Swift;
    public bool UltraSwift;

    public SwiftDetails(StageEntity owner, bool swift, bool ultraSwift)
    {
        Owner = owner;
        Swift = swift;
        UltraSwift = ultraSwift;
    }
}
