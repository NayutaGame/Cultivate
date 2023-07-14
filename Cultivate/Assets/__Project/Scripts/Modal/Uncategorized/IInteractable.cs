using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractDelegate GetDelegate();
    void SetDelegate(InteractDelegate interactDelegate);

    IndexPath GetIndexPath();
}
