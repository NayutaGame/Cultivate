
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace CLLibrary
{
    public class Neuron
    {
        private Action Action;

        public void ClearAction() => Action = null;

        public void Add(Action action) => Action += action;
        public void Remove(Action action) => Action -= action;
        public void Join(params Action[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron neuron) => _neurons.Add(neuron);
        public void Remove(Neuron neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke()
        {
            if (!Active) return;
            Action?.Invoke();
            _neurons.Do(neuron => neuron.Invoke());
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }
    
    public class AsyncNeuron
    {
        private Func<UniTask> Action;

        public void ClearAction() => Action = null;

        public void Add(Func<UniTask> action) => Action += action;
        public void Remove(Func<UniTask> action) => Action -= action;
        public void Join(params Func<UniTask>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<AsyncNeuron> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(AsyncNeuron neuron) => _neurons.Add(neuron);
        public void Remove(AsyncNeuron neuron) => _neurons.Remove(neuron);
        public void Join(params AsyncNeuron[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public async UniTask Invoke()
        {
            if (!Active) return;
            if (Action != null)
                await Action.Invoke();
            await _neurons.Do(async neuron => await neuron.Invoke());
        }

        public bool Active;

        public AsyncNeuron()
        {
            Active = true;
            _neurons = new();
        }
    }

    public class Neuron<T1>
    {
        private Action<T1> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1> action) => Action += action;
        public void Remove(Action<T1> action) => Action -= action;
        public void Join(params Action<T1>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron<T1>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1> neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron<T1>[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke(T1 t1)
        {
            if (!Active) return;
            Action?.Invoke(t1);
            _neurons.Do(neuron => neuron.Invoke(t1));
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }

    public class Neuron<T1, T2>
    {
        private Action<T1, T2> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2> action) => Action += action;
        public void Remove(Action<T1, T2> action) => Action -= action;
        public void Join(params Action<T1, T2>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron<T1, T2>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2> neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron<T1, T2>[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke(T1 t1, T2 t2)
        {
            if (!Active) return;
            Action?.Invoke(t1, t2);
            _neurons.Do(neuron => neuron.Invoke(t1, t2));
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3>
    {
        private Action<T1, T2, T3> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3> action) => Action += action;
        public void Remove(Action<T1, T2, T3> action) => Action -= action;
        public void Join(params Action<T1, T2, T3>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron<T1, T2, T3>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3> neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron<T1, T2, T3>[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke(T1 t1, T2 t2, T3 t3)
        {
            if (!Active) return;
            Action?.Invoke(t1, t2, t3);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3));
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3, T4>
    {
        private Action<T1, T2, T3, T4> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3, T4> action) => Action += action;
        public void Remove(Action<T1, T2, T3, T4> action) => Action -= action;
        public void Join(params Action<T1, T2, T3, T4>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron<T1, T2, T3, T4>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3, T4> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3, T4> neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron<T1, T2, T3, T4>[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!Active) return;
            Action?.Invoke(t1, t2, t3, t4);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3, t4));
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3, T4, T5>
    {
        private Action<T1, T2, T3, T4, T5> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3, T4, T5> action) => Action += action;
        public void Remove(Action<T1, T2, T3, T4, T5> action) => Action -= action;
        public void Join(params Action<T1, T2, T3, T4, T5>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron<T1, T2, T3, T4, T5>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3, T4, T5> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3, T4, T5> neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron<T1, T2, T3, T4, T5>[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (!Active) return;
            Action?.Invoke(t1, t2, t3, t4, t5);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3, t4, t5));
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3, T4, T5, T6>
    {
        private Action<T1, T2, T3, T4, T5, T6> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3, T4, T5, T6> action) => Action += action;
        public void Remove(Action<T1, T2, T3, T4, T5, T6> action) => Action -= action;
        public void Join(params Action<T1, T2, T3, T4, T5, T6>[] actions)
            => actions.FilterObj(action => Action == null || !Action.GetInvocationList().Contains(action)).Do(action => Action += action);

        private List<Neuron<T1, T2, T3, T4, T5, T6>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3, T4, T5, T6> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3, T4, T5, T6> neuron) => _neurons.Remove(neuron);
        public void Join(params Neuron<T1, T2, T3, T4, T5, T6>[] neurons)
            => neurons.FilterObj(n => !_neurons.Contains(n)).Do(_neurons.Add);

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (!Active) return;
            Action?.Invoke(t1, t2, t3, t4, t5, t6);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3, t4, t5, t6));
        }

        public bool Active;

        public Neuron()
        {
            Active = true;
            _neurons = new();
        }
    }
}
