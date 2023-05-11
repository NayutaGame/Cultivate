using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractDelegate GetInteractDelegate();
    void SetInteractDelegate(InteractDelegate interactDelegate);
}
