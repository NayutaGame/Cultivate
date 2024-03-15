
using UnityEngine;

public class SpriteEntry : Entry
{
    private string _path;
    public Sprite Sprite { get; private set; }

    public SpriteEntry(string id, string path) : base(id)
    {
        _path = path;
        Sprite = Resources.Load<Sprite>(_path);
    }

    public static implicit operator SpriteEntry(string id) => Encyclopedia.SpriteCategory[id];
}
