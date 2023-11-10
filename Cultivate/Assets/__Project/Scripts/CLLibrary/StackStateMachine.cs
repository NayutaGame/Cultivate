
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                if (_stack.Count >= 1)
                    return _stack[^1];
                return null;
            }
        }

        public S SecondCurrent
        {
            get
            {
                if (_stack.Count >= 2)
                    return _stack[^2];
                return null;
            }
        }

        public StackStateMachine()
        {
            _stack = new List<S>();
        }

        public async Task Push(S state)
        {
            // StackChangeDetails<SM, S> d = new StackChangeDetails<SM, S>(Current, state);
            if (Current != null)
                await Current.CEnter();
            _stack.Add(state);
            await Current.Enter();
        }

        public async Task Pop()
        {
            // StackChangeDetails<SM, S> d = new StackChangeDetails<SM, S>(Current, SecondCurrent);
            await Current.Exit();
            _stack.RemoveAt(_stack.Count - 1);
            if (Current != null)
                await Current.CExit();
        }
    }
}
