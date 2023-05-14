using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLLibrary
{
    public class Pool<T>
    {
        private List<T> _list;

        public Pool()
        {
            _list = new List<T>();
        }

        public void Populate(IEnumerable<T> list)
        {
            _list.AddRange(list);
        }

        public void Populate(IEnumerable<T> list, Predicate<T> pred)
        {
            _list.AddRange(list.FilterObj(pred));
        }

        public void Shuffle()
        {
            for (int i = _list.Count; i > 0; i--)
            {
                int r = RandomManager.Range(0, i);
                T item = _list[r];
                _list.RemoveAt(r);
                _list.Add(item);
            }
        }

        public bool TryPopItem(out T item)
        {
            if (_list.Count == 0)
            {
                item = default;
                return false;
            }
            else
            {
                item = _list[0];
                _list.RemoveAt(0);
                return true;
            }
        }

        public bool TryPopItem(out T item, Predicate<T> pred)
        {
            item = _list.FirstObj(pred);
            if (item == null) return false;
            _list.Remove(item);
            return true;
        }

        public int Count() => _list.Count;
    }
}
