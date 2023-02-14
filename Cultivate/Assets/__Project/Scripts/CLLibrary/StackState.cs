
namespace CLLibrary
{
    public class StackState<SM, S>
        where SM : StackStateMachine<SM, S>
        where S : StackState<SM, S>
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void CEnter() { }
        public virtual void CExit() { }
    }
}
