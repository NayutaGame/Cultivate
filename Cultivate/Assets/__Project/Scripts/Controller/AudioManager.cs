
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
}
