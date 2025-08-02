
using UnityEngine;

public class SpriteEntry : Entry
{
    private string _path;
    public Sprite Sprite { get; private set; }

    public SpriteEntry(string id, string name, string path) : base(id, name)
    {
        _path = path;
        Sprite = Resources.Load<Sprite>(_path);
    }
}
