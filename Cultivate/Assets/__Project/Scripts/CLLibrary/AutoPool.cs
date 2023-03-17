using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLLibrary
{
    public class AutoPool<T> : Pool<T>
    {
        private List<T> _toPopulate;

        public AutoPool(List<T> toPopulate)
        {
            _toPopulate = toPopulate;
        }

        public T ForcePopItem()
        {
            if (!TryPopItem(out T item))
            {
                Populate(_toPopulate);
                Shuffle();
                TryPopItem(out item);
            }

            return item;
        }

        public T ForcePopItem(Predicate<T> pred)
        {
            if (!TryPopItem(pred, out T item))
            {
                Populate(_toPopulate);
                Shuffle();
                TryPopItem(pred, out item);
            }

            return item;
        }
    }
}
