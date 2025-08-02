
using System.Collections.Generic;

public interface ICategory<out T> : IEnumerable<T> where T : Entry
{
    bool ContainsName(string name);
    T FromName(string name);
    void Init();
}