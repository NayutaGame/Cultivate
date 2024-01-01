
using System;
using UnityEngine;

[RequireComponent(typeof(LegacyAddressBehaviour))]
public class ItemView : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;
    public LegacyAddressBehaviour AddressBehaviour;
}
