
using System;
using System.Collections.Generic;

public class Memory
{
    private Dictionary<string, object> _memory;

    public void SetVariable<T>(string key, T value)
        => _memory[key] = value;

    public T TryGetVariable<T>(string key, T defaultValue)
    {
        _memory.TryAdd(key, defaultValue);
        return (T)_memory[key];
    }

    public void PerformOperation<T>(string key, T defaultValue, Func<T, T> operation)
    {
        T value = TryGetVariable(key, defaultValue);
        value = operation(value);
        SetVariable(key, value);
    }

    public Memory()
    {
        _memory = new();
    }
}
