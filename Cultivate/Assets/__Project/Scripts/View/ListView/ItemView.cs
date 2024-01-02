
using System;
using UnityEngine;

[RequireComponent(typeof(AddressBehaviour))]
public class ItemView : MonoBehaviour
{
    [NonSerialized] public int PrefabIndex;
    public AddressBehaviour AddressBehaviour;
}
