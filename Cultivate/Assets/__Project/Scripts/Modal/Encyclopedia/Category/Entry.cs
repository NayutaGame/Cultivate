
using UnityEngine;

public class Entry
{
    [SerializeField] private string _name;
    public string Name => _name;

    public Entry(string name)
    {
        _name = name;
    }
}
