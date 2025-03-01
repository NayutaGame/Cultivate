
using System;
using System.Collections.Generic;

[Serializable]
public class Memory
{
    // 需要序列化，但是目前无法序列化
    private Dictionary<string, object> _memory;

    public void SetVariable<T>(string key, T value)
        => _memory[key] = value;

    public T TryGetVariable<T>(string key, T defaultValue)
    {
        _memory.TryAdd(key, defaultValue);
        return (T)_memory[key];
    }

    public T PerformOperation<T>(string key, T defaultValue, Func<T, T> operation)
    {
        T value = TryGetVariable(key, defaultValue);
        value = operation(value);
        SetVariable(key, value);
        return value;
    }

    public Memory()
    {
        _memory = new();
    }
}
