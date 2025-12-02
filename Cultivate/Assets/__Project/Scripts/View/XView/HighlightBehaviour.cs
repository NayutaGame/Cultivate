
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(XView))]
public class HighlightBehaviour : XBehaviour
{
    [SerializeField] private Image EffectImage;
    private Material _outlineMaterial;
    private static readonly int OuterOutlineFade = Shader.PropertyToID("_OuterOutlineFade");
    
    private Tween _handle;
    
    private bool _highlight;
    public bool IsHighlighted => _highlight;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        // 初始化材质
        if (EffectImage != null)
        {
            EffectImage.material = Instantiate(EffectImage.material);
            _outlineMaterial = EffectImage.materialForRendering;
        }
    }

    public void SetHighlight(bool highlight)
    {
        if (_highlight == highlight) return;
        _highlight = highlight;
        
        _handle?.Kill();
        _handle = DOTween.To(GetOutlineFade, SetOutlineFade, _highlight ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad);
        _handle.SetAutoKill().Restart();
    }

    private float GetOutlineFade() => _outlineMaterial.GetFloat(OuterOutlineFade);
    private void SetOutlineFade(float value) => _outlineMaterial.SetFloat(OuterOutlineFade, value);

    private void OnDestroy()
    {
        _handle?.Kill();
        if (_outlineMaterial != null)
        {
            Destroy(_outlineMaterial);
        }
    }
}
