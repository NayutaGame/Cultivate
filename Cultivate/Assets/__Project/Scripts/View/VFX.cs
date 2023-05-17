using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX : MonoBehaviour
{
    private VisualEffect _ve;
    private AudioSource _audio;

    private void Awake()
    {
        _ve = GetComponentInChildren<VisualEffect>();
        _audio = GetComponent<AudioSource>();
    }

    public void SetIntensity(float intensity)
    {
        transform.localScale = Mathf.Lerp(0.4f, 0.6f, intensity) * Vector3.one;
        _audio.volume = Mathf.Lerp(0.2f, 1f, intensity);
    }

    public void Play()
    {
        _ve.Play();
        _audio.Play();
        Destroy(gameObject, 5);
    }
}
