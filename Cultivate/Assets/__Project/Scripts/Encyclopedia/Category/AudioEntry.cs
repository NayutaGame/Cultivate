
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

    public AudioEntry(string id, string name, string path, AudioType audioType) : base(id, name)
    {
        _path = path;
        EventReference = RuntimeManager.PathToEventReference(path);
        _audioType = audioType;
    }
}
