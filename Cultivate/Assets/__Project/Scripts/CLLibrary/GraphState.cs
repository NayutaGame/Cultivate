
namespace CLLibrary
{
    public abstract class GraphState<SM, S>
        where SM : GraphStateMachine<SM, S>
        where S : GraphState<SM, S>
    {
        protected SM _stateMachine { get; }
        protected GraphState(SM stateMachine) => _stateMachine = stateMachine;

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
