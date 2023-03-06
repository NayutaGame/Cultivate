using System;

namespace CLLibrary
{
    public class Dirty<T>
    {
        protected Func<T> _generator;
        protected bool _dirty;
        protected T _value;

        public Dirty(Func<T> generator)
        {
            _generator = generator;
            _dirty = true;
        }

        public virtual T Value
        {
            get
            {
                if (!_dirty) return _value;

                _value = _generator();
                _dirty = false;
                return _value;
            }
        }

        public virtual bool IsDirty()
        {
            return _dirty;
        }

        public virtual void SetDirty()
        {
            _dirty = true;
        }
    }
}
