
using UnityEngine;

public class PrefabEntry : Entry
{
    private string _path;
    public GameObject Prefab { get; private set; }

    public PrefabEntry(string id, string name, string path) : base(id, name)
    {
        _path = path;
        Prefab = Resources.Load<GameObject>(_path);
    }
}
