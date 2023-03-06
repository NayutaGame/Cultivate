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
            list.Do(t => _list.Add(t));
        }

        public void Populate(IEnumerable<T> list, Predicate<T> pred)
        {
            list.FilterObj(pred).Do(t => _list.Add(t));
        }

        public void Shuffle()
        {
            List<T> temp = new List<T>(_list);
            _list.Clear();

            while (temp.Count > 0)
            {
                int r = RandomManager.Range(0, temp.Count);
                _list.Add(temp[r]);
                temp.RemoveAt(r);
            }
        }

        public bool TryPopFirst(Predicate<T> pred, out T item)
        {
            item = _list.FirstObj(pred);
            if (item == null) return false;
            _list.Remove(item);
            return true;
        }
    }
}
