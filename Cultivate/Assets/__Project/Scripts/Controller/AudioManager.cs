
using CLLibrary;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : Singleton<AudioManager>
{
    private Bus MasterBus;
    private Bus MusicBus;
    private Bus SFXBus;

    public override void DidAwake()
    {
        base.DidAwake();

        MasterBus = RuntimeManager.GetBus("bus:/");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");

        SetAudible(true);
    }

    private EventInstance BGMEventInstance;

    public static void Play(AudioEntry audioEntry)
    {
        if (audioEntry._audioType == AudioEntry.AudioType.Music)
        {
            Instance.PlayMusic(audioEntry);
        }
        else if (audioEntry._audioType == AudioEntry.AudioType.SFX)
        {
            Instance.PlaySFX(audioEntry);
        }
    }

    public void Stop()
    {
        BGMEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void PlaySFX(AudioEntry audioEntry)
    {
        RuntimeManager.PlayOneShot(audioEntry.EventReference);
    }

    private void PlayMusic(AudioEntry audioEntry)
    {
        // BGMEventInstance.release();
        BGMEventInstance = RuntimeManager.CreateInstance(audioEntry.EventReference);
        BGMEventInstance.start();
    }

    private bool _audible;
    public bool IsAudible => _audible;
    public void SetAudible(bool audible)
    {
        _audible = audible;
        // _audioSource.volume = _audible ? 1 : 0;
    }

    public void ToggleAudible()
    {
        SetAudible(!IsAudible);
    }

    public void SetMasterVolume(int value)
    {
        MasterBus.setVolume(value);
    }

    public void SetMusicVolume(int value)
    {
        MusicBus.setVolume(value);
    }

    public void SetSFXVolume(int value)
    {
        SFXBus.setVolume(value);
    }
}
