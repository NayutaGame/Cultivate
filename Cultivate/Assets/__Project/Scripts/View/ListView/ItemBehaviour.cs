
using System;
using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;

    public abstract AddressBehaviour GetAddressBehaviour();
    public abstract SelectBehaviour GetSelectBehaviour();
    public abstract InteractBehaviour GetInteractBehaviour();
    public abstract void RefreshPivots();
}
