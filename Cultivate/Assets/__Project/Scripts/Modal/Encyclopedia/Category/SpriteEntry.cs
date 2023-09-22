using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEntry : Entry
{
    private string _path;
    public Sprite Sprite { get; private set; }

    public SpriteEntry(string name, string path) : base(name)
    {
        _path = path;
        Sprite = Resources.Load<Sprite>(_path);
    }

    public static implicit operator SpriteEntry(string name) => Encyclopedia.SpriteCategory[name];
}
