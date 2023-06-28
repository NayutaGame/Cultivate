
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CLLibrary;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _audioSource;

    public override void DidAwake()
    {
        base.DidAwake();

        SetAudible(true);
    }

    public void Play(AudioEntry audioEntry)
    {
        if (audioEntry._audioType == AudioEntry.AudioType.Music)
        {
            PlayMusic(audioEntry);
        }
        else if (audioEntry._audioType == AudioEntry.AudioType.SFX)
        {
            PlaySFX(audioEntry);
        }
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    private void PlaySFX(AudioEntry audioEntry)
    {
        _audioSource.PlayOneShot(audioEntry.AudioClip);
    }

    private void PlayMusic(AudioEntry audioEntry)
    {
        _audioSource.clip = audioEntry.AudioClip;
        _audioSource.Play();
    }

    private bool _audible;
    public bool IsAudible => _audible;
    public void SetAudible(bool audible)
    {
        _audible = audible;
        _audioSource.volume = _audible ? 1 : 0;
    }

    public void ToggleAudible()
    {
        SetAudible(!IsAudible);
    }
}
