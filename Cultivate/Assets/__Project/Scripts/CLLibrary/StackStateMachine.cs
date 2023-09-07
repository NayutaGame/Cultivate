
using System.Collections.Generic;

namespace CLLibrary
{
    public class StackStateMachine<SM, S>
        where SM : StackStateMachine<SM, S>
        where S : StackState<SM, S>
    {
        private List<S> _stack;

        public S Current
        {
            get
            {
                if (_stack.Count > 0)
                    return _stack[^1];
                return null;
            }
        }

        public StackStateMachine()
        {
            _stack = new List<S>();
        }

        public void Push(S state)
        {
            if (Current != null)
                Current.CEnter();
            _stack.Add(state);
            Current.Enter();
        }

        public void Pop()
        {
            Current.Exit();
            _stack.RemoveAt(_stack.Count - 1);
            if (Current != null)
                Current.CExit();
        }
    }
}
