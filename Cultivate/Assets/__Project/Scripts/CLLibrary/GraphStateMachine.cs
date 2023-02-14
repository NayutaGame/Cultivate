
namespace CLLibrary
{
    public abstract class GraphStateMachine<SM, S>
        where SM : GraphStateMachine<SM, S>
        where S : GraphState<SM, S>
    {
        public S Current { get; private set; }

        protected S[] _states;
        public S this[int i] => _states[i];

        public virtual void ChangeState(int i) => ChangeState(_states[i]);
        public virtual void ChangeState(S state)
        {
            Current?.Exit();
            Current = state;
            Current?.Enter();
        }

        public virtual void SetState(int i) => SetState(_states[i]);
        public virtual void SetState(S state) => Current = state;

        public virtual void Update() => Current?.Update();
        public virtual void FixedUpdate() => Current?.FixedUpdate();
    }
}
