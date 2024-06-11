
namespace CLLibrary
{
    public class Table<T>
    {
        public int _size;
        public int Size => _size;
    
        private T[,] _content;
        public T this[int from, int to]
        {
            get => _content[Convert(from), Convert(to)];
            set => _content[Convert(from), Convert(to)] = value;
        }

        private int Convert(int i)
            => i == -1 ? _size : i;

        public Table(int size)
        {
            _size = size;
            _content = new T[_size + 1, _size + 1];
        }
    }
}
