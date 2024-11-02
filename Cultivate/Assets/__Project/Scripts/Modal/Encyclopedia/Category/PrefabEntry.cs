
using UnityEngine;

public class PrefabEntry : Entry
{
    private string _path;
    public GameObject Prefab { get; private set; }

    public PrefabEntry(string id, string path) : base(id)
    {
        _path = path;
        Prefab = Resources.Load<GameObject>(_path);
    }

    public static implicit operator PrefabEntry(string id) => Encyclopedia.PrefabCategory[id];
}
