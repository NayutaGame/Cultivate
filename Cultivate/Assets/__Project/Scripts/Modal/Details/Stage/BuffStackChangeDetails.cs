using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStackChangeDetails : ClosureDetails
{
    public int _oldStack;
    public int _newStack;

    public BuffStackChangeDetails(int oldStack, int newStack)
    {
        _oldStack = oldStack;
        _newStack = newStack;
    }
}
