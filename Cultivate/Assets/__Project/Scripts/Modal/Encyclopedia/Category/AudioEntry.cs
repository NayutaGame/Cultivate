using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEntry : Entry
{
    private string _path;
    public AudioClip AudioClip { get; private set; }
    public AudioType _audioType { get; private set; }

    public enum AudioType
    {
        Music,
        SFX,
    }

    public AudioEntry(string name, string path, AudioType audioType) : base(name)
    {
        _path = path;
        AudioClip = Resources.Load<AudioClip>(_path);
        _audioType = audioType;
    }

    public static implicit operator AudioEntry(string name) => Encyclopedia.AudioCategory[name];
}
