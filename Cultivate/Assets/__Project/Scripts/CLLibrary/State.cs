using UnityEngine;

namespace CLLibrary
{
    public abstract class State<SM, S>
        where SM : StateMachine<SM, S>
        where S : State<SM, S>
    {
        protected SM _stateMachine { get; }
        protected float _startTime { get; private set; }
        protected State(SM stateMachine) => _stateMachine = stateMachine;

        public virtual void Enter() => _startTime = Time.time;
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnDrawGizmos() { }
    }
}
