
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
    }

    public void Play(SoundEntry soundEntry)
    {
        if (soundEntry._audioType == SoundEntry.AudioType.Music)
        {
            PlayMusic(soundEntry);
        }
        else if (soundEntry._audioType == SoundEntry.AudioType.SFX)
        {
            PlaySFX(soundEntry);
        }
    }

    private void PlaySFX(SoundEntry soundEntry)
    {
        _audioSource.PlayOneShot(soundEntry.AudioClip);
    }

    private void PlayMusic(SoundEntry soundEntry)
    {
        _audioSource.clip = soundEntry.AudioClip;
        _audioSource.Play();
    }
}
