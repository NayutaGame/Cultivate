using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public interface IInventory
{
    int GetCount();
    string GetIndexPathString();
}
