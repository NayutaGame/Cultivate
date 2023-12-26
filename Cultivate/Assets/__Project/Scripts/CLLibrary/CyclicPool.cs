
using System;
using System.Collections.Generic;

namespace CLLibrary
{
    public class CyclicPool<T> : Pool<T>
    {
        private List<T> _generator;
        public List<T> Generator => _generator;

        public CyclicPool(List<T> generator)
        {
            _generator = generator;
        }

        public T ForcePopItem()
        {
            if (!TryPopItem(out T item))
            {
                Populate(_generator);
                Shuffle();
                TryPopItem(out item);
            }

            return item;
        }

        public T ForcePopItem(Predicate<T> pred)
        {
            if (!TryPopItem(out T item, pred))
            {
                Populate(_generator);
                Shuffle();
                TryPopItem(out item, pred);
            }

            return item;
        }
    }
}
