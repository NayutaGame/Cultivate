using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEntry : Entry
{
    private string _path;
    public AudioClip AudioClip { get; private set; }
    public AudioType _audioType { get; private set; }

    public enum AudioType
    {
        Music,
        SFX,
    }

    public SoundEntry(string name, string path, AudioType audioType) : base(name)
    {
        _path = path;
        AudioClip = Resources.Load<AudioClip>(_path);
        _audioType = audioType;
    }

    public static implicit operator SoundEntry(string name) => Encyclopedia.SoundCategory[name];
}
