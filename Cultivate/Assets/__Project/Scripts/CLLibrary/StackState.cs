
using System.Threading.Tasks;

namespace CLLibrary
{
    public class StackState<SM, S>
        where SM : StackStateMachine<SM, S>
        where S : StackState<SM, S>
    {
        public virtual async Task Enter() { }
        public virtual async Task Exit() { }
        public virtual async Task CEnter() { }
        public virtual async Task CExit() { }
    }
}
