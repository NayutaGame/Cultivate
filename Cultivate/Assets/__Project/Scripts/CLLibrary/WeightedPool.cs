
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CLLibrary
{
    public class WeightedPool<T>
    {
        [SerializeReference] private List<WeightedItem<T>> _list;
        [NonSerialized] private int _totalWeight;

        public WeightedPool()
        {
            _list = new();
            _totalWeight = 0;
        }

        public void Populate(int weight, T item)
        {
            _list.Add(new(weight, item));
            _totalWeight += weight;
        }

        public bool Draw(out T item)
        {
            if (_list.Count == 0)
            {
                item = default(T);
                return false;
            }

            if (_totalWeight <= 0)
            {
                item = default(T);
                return false;
            }

            // 随机生成一个权重值
            int randomWeight = UnityEngine.Random.Range(0, _totalWeight);
            int currentWeight = 0;

            // 根据权重选择物品
            foreach (var entry in _list)
            {
                currentWeight += entry.Weight;
                if (randomWeight < currentWeight)
                {
                    item = entry.Item;
                    return true;
                }
            }

            item = _list[_list.Count - 1].Item;
            return true;
        }

        public int Count() => _list.Count;
    }
}
