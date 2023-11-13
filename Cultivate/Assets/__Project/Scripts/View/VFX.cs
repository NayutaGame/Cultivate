
using UnityEngine;
using UnityEngine.VFX;

public class VFX : MonoBehaviour
{
    private VisualEffect _ve;
    private ParticleSystem _ps;

    private void Awake()
    {
        _ve = GetComponentInChildren<VisualEffect>();
        _ps = GetComponent<ParticleSystem>();
        // _audio = GetComponent<AudioSource>();
    }

    public void SetIntensity(float intensity)
    {
        if (_ve != null)
        {
            transform.localScale = Mathf.Lerp(0.4f, 0.6f, intensity) * Vector3.one;
        }
        // _audio.volume = Mathf.Lerp(0.2f, 1f, intensity);
    }

    public void Play()
    {
        // _audio.Play();
        if (_ve != null)
        {
            _ve.Play();
            Destroy(gameObject, 5);
        }
        if (_ps != null)
        {
            _ps.Play();
            Destroy(gameObject, _ps.main.duration);
        }
    }
}
