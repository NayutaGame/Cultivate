
using System.Threading.Tasks;

namespace CLLibrary
{
    public class CommandStateMachine : StateMachine<Command>
    {
        public CommandStateMachine(int size) : base(size)
        {
        }

        public override void SetState(int value)
        {
            GetElement(GetState(), GetSize())?.Execute();
            GetElement(GetState(), value)?.Execute();
            base.SetState(value);
            GetElement(GetSize(), GetState())?.Execute();
        }

        public async Task AsyncSetState(int value)
        {
            Command c;
            
            c = GetElement(GetState(), GetSize());
            if (c != null)
                await c.AsyncExecute();

            c = GetElement(GetState(), value);
            if (c != null)
                await c.AsyncExecute();
            
            base.SetState(value);
            
            c = GetElement(GetSize(), GetState());
            if (c != null)
                await c.AsyncExecute();
        }
    }
}
