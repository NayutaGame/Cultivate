
using UnityEngine;

public class Entry
{
    [SerializeField] private string _id;
    public string GetId() => _id;

    public Entry(string id)
    {
        _id = id;
    }
}
