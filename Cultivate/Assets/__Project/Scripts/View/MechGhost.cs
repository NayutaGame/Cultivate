using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechGhost : MechView
{
    public void UpdateMousePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }
}
