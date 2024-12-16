
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReactionView : MonoBehaviour
{
    [SerializeField] private Image ReactionImage;

    private float _intensity;

    public float GetIntensity() => _intensity;
    
    public void SetIntensity(float intensity)
    {
        _intensity = intensity;
        ReactionImage.color = new Color(1, 1, 1, intensity);
        ReactionImage.transform.position = transform.position + Vector3.up * 0.5f * (1 - intensity);
    }

    private float _targetIntensity;
    public float GetTargetIntensity() => _targetIntensity;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private Tween _handle;
    public bool IsAnimating => _handle != null && _handle.active;

    public void BeginDrag(Sprite sprite, float targetIntensity)
    {
        ReactionImage.gameObject.SetActive(sprite != null);
        if (sprite == null)
            return;
        ReactionImage.sprite = sprite;

        _targetIntensity = targetIntensity;
        
        _handle?.Kill();
        _handle = new FloatAnimation(SetIntensity, GetIntensity, GetTargetIntensity).GetHandle();
        _handle.Restart();
    }

    public void EndDrag()
    {
        _targetIntensity = 0;
        
        _handle?.Kill();
        _handle = new FloatAnimation(SetIntensity, GetIntensity, GetTargetIntensity).GetHandle();
        _handle.Restart();
    }

    public void Drag(float targetIntensity)
    {
        _targetIntensity = targetIntensity;
        if (IsAnimating)
            return;
        SetIntensity(_targetIntensity);
    }
}
