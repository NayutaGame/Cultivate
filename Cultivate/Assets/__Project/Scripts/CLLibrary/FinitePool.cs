
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CLLibrary
{
    [Serializable]
    public class FinitePool<T>
    {
        [SerializeReference] private List<T> _list;

        public FinitePool()
        {
            _list = new();
        }

        public void Populate(T item)
        {
            _list.Add(item);
        }

        public void Populate(IEnumerable<T> list)
        {
            _list.AddRange(list);
        }

        public void Populate(IEnumerable<T> list, Predicate<T> pred)
        {
            _list.AddRange(list.FilterObj(pred));
        }

        public void Depopulate(Predicate<T> pred)
        {
            _list.RemoveAll(pred);
        }

        public void Shuffle()
        {
            _list.Shuffle();
        }

        public bool TryPeekItem(out T item, Predicate<T> pred = null)
        {
            item = _list.FirstObj(pred ?? (t => true));
            if (item == null)
                return false;

            return true;
        }

        public bool TryPopItem(out T item, Predicate<T> pred = null)
        {
            item = _list.FirstObj(pred ?? (t => true));
            if (item == null) return false;
            _list.Remove(item);
            return true;
        }

        public int Count() => _list.Count;
    }
}
