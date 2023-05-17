using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private ParticleSystem _ps;
    private AudioSource _audio;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
    }

    public void SetIntensity(float intensity)
    {
        transform.localScale = Mathf.Lerp(0.1f, 0.25f, intensity) * Vector3.one;
        _audio.volume = Mathf.Lerp(0.2f, 1f, intensity);
    }

    public void Play()
    {
        _ps.Play();
        _audio.Play();
        Destroy(gameObject, _ps.main.duration);
    }
}
