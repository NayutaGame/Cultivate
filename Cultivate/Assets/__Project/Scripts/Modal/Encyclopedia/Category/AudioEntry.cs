
using FMODUnity;

public class AudioEntry : Entry
{
    private string _path;
    public EventReference EventReference { get; private set; }
    public AudioType _audioType { get; private set; }

    public enum AudioType
    {
        Music,
        SFX,
    }

    public AudioEntry(string name, string path, AudioType audioType) : base(name)
    {
        _path = path;
        EventReference = RuntimeManager.PathToEventReference(path);
        _audioType = audioType;
    }

    public static implicit operator AudioEntry(string name) => Encyclopedia.AudioCategory[name];
}
