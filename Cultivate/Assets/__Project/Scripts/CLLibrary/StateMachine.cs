
namespace CLLibrary
{
    public class StateMachine<T>
    {
        private int _state;
        public virtual int GetState() => _state;
        public virtual void SetState(int value) => _state = value;

        private T[,] _table;
        public T this[int from, int to]
        {
            get => _table[from, to];
            set => _table[from, to] = value;
        }
        public virtual T GetElement(int from, int to) => this[from, to];
        public virtual void SetElement(int from, int to, T value) => this[from, to] = value;

        private int _size;
        public int GetSize() => _size;
        
        public StateMachine(int size)
        {
            _size = size;
            _table = new T[_size + 1, _size + 1];
        }
    }
}
