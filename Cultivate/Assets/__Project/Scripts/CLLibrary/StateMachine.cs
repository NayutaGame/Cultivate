
namespace CLLibrary
{
    public class StateMachine<T>
    {
        private T _state;
        public T State => _state;

        public virtual void SetState(T value)
        {
            _state = value;
        }
    }
}
