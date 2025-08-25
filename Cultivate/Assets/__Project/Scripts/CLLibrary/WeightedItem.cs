
using System;
using UnityEngine;

namespace CLLibrary
{
    [Serializable]
    public class WeightedItem<T>
    {
        [SerializeField] public int Weight;
        [SerializeReference] public T Item;

        public WeightedItem(int weight, T item)
        {
            Weight = weight;
            Item = item;
        }
    }
}