
using System;
using System.Threading.Tasks;
using DG.Tweening;

namespace CLLibrary
{
    public class TableSM : StateMachine<int>
    {
        private Table<Func<Tween>> _table;
        public Func<Tween> this[int from, int to]
        {
            get => _table[from, to];
            set => _table[from, to] = value;
        }

        public Tween SetStateTween(int value)
        {
            Sequence seq = DOTween.Sequence();
            
            if (_table[State, -1] is { } first)
                seq.Append(first());

            if (_table[State, value] is { } second)
                seq.Append(second());

            seq.AppendCallback(() => base.SetState(value));

            if (_table[-1, State] is { } third)
                seq.Append(third());

            return seq;
        }

        public void SetState(int value, Action<Func<Tween>> activator)
        {
            activator(_table[State, -1]);
            activator(_table[State, value]);
            base.SetState(value);
            activator(_table[-1, State]);
        }

        public async Task SetStateAsync(int value, Func<Func<Tween>, Task> activator)
        {
            await activator(_table[State, -1]);
            await activator(_table[State, value]);
            base.SetState(value);
            await activator(_table[-1, State]);
        }

        public TableSM(int size)
        {
            _table = new(size);
        }
    }
}
