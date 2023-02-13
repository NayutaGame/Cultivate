namespace CLLibrary
{
    public abstract class StateMachine<SM, S>
        where SM : StateMachine<SM, S>
        where S : State<SM, S>
    {
        public S _current { get; private set; }

        protected S[] _states;
        public S this[int i] => _states[i];

        public virtual void ChangeState(int i) => ChangeState(_states[i]);
        public virtual void ChangeState(S state)
        {
            if (_current != null)
            {
                _current.Exit();
                PostExit();
            }

            _current = state;
            if (_current != null)
            {
                PreEnter();
                _current.Enter();
            }
        }

        public virtual void SetState(int i) => SetState(_states[i]);
        public virtual void SetState(S state) => _current = state;

        public virtual void PostExit() { }
        public virtual void PreEnter() { }

        public virtual void Update() => _current.Update();

        public virtual void FixedUpdate() => _current.FixedUpdate();

        public virtual void OnDrawGizmos()
        {
            foreach(S state in _states) state.OnDrawGizmos();
        }
    }
}
