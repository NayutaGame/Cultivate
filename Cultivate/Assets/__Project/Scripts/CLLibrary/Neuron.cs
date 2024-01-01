
using System;
using System.Collections.Generic;

namespace CLLibrary
{
    public class Neuron
    {
        private Action Action;

        public void ClearAction() => Action = null;

        public void Add(Action action) => Action += action;
        public void Remove(Action action) => Action -= action;
        public void Set(params Action[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron neuron) => _neurons.Add(neuron);
        public void Remove(Neuron neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke()
        {
            Action?.Invoke();
            _neurons.Do(neuron => neuron.Invoke());
        }

        public Neuron()
        {
            _neurons = new();
        }
    }

    public class Neuron<T1>
    {
        private Action<T1> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1> action) => Action += action;
        public void Remove(Action<T1> action) => Action -= action;
        public void Set(params Action<T1>[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron<T1>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1> neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron<T1>[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke(T1 t1)
        {
            Action?.Invoke(t1);
            _neurons.Do(neuron => neuron.Invoke(t1));
        }

        public Neuron()
        {
            _neurons = new();
        }
    }

    public class Neuron<T1, T2>
    {
        private Action<T1, T2> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2> action) => Action += action;
        public void Remove(Action<T1, T2> action) => Action -= action;
        public void Set(params Action<T1, T2>[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron<T1, T2>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2> neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron<T1, T2>[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke(T1 t1, T2 t2)
        {
            Action?.Invoke(t1, t2);
            _neurons.Do(neuron => neuron.Invoke(t1, t2));
        }

        public Neuron()
        {
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3>
    {
        private Action<T1, T2, T3> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3> action) => Action += action;
        public void Remove(Action<T1, T2, T3> action) => Action -= action;
        public void Set(params Action<T1, T2, T3>[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron<T1, T2, T3>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3> neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron<T1, T2, T3>[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke(T1 t1, T2 t2, T3 t3)
        {
            Action?.Invoke(t1, t2, t3);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3));
        }

        public Neuron()
        {
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3, T4>
    {
        private Action<T1, T2, T3, T4> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3, T4> action) => Action += action;
        public void Remove(Action<T1, T2, T3, T4> action) => Action -= action;
        public void Set(params Action<T1, T2, T3, T4>[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron<T1, T2, T3, T4>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3, T4> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3, T4> neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron<T1, T2, T3, T4>[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Action?.Invoke(t1, t2, t3, t4);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3, t4));
        }

        public Neuron()
        {
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3, T4, T5>
    {
        private Action<T1, T2, T3, T4, T5> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3, T4, T5> action) => Action += action;
        public void Remove(Action<T1, T2, T3, T4, T5> action) => Action -= action;
        public void Set(params Action<T1, T2, T3, T4, T5>[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron<T1, T2, T3, T4, T5>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3, T4, T5> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3, T4, T5> neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron<T1, T2, T3, T4, T5>[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            Action?.Invoke(t1, t2, t3, t4, t5);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3, t4, t5));
        }

        public Neuron()
        {
            _neurons = new();
        }
    }

    public class Neuron<T1, T2, T3, T4, T5, T6>
    {
        private Action<T1, T2, T3, T4, T5, T6> Action;

        public void ClearAction() => Action = null;

        public void Add(Action<T1, T2, T3, T4, T5, T6> action) => Action += action;
        public void Remove(Action<T1, T2, T3, T4, T5, T6> action) => Action -= action;
        public void Set(params Action<T1, T2, T3, T4, T5, T6>[] actions)
        {
            Action = null;
            actions.Do(action => Action += action);
        }

        private List<Neuron<T1, T2, T3, T4, T5, T6>> _neurons;

        public void Clear() => _neurons.Clear();

        public void Add(Neuron<T1, T2, T3, T4, T5, T6> neuron) => _neurons.Add(neuron);
        public void Remove(Neuron<T1, T2, T3, T4, T5, T6> neuron) => _neurons.Remove(neuron);
        public void Set(params Neuron<T1, T2, T3, T4, T5, T6>[] neurons)
        {
            _neurons.Clear();
            _neurons.AddRange(neurons);
        }

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            Action?.Invoke(t1, t2, t3, t4, t5, t6);
            _neurons.Do(neuron => neuron.Invoke(t1, t2, t3, t4, t5, t6));
        }

        public Neuron()
        {
            _neurons = new();
        }
    }
}
