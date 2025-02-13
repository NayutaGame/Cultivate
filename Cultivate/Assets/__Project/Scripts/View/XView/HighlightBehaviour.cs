
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(XView))]
public class HighlightBehaviour : XBehaviour
{
    [SerializeField] private Image EffectImage;
    private Material _outlineMaterial;
    private Tween _highlightHandle;
    private static readonly int OuterOutlineFade = Shader.PropertyToID("_OuterOutlineFade");
    
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
        
        _highlightHandle?.Kill();
        _highlightHandle = DOTween.To(GetOutlineFade, SetOutlineFade, _highlight ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad);
        _highlightHandle.SetAutoKill().Restart();
    }

    private float GetOutlineFade() => _outlineMaterial.GetFloat(OuterOutlineFade);
    private void SetOutlineFade(float value) => _outlineMaterial.SetFloat(OuterOutlineFade, value);

    private void OnDestroy()
    {
        _highlightHandle?.Kill();
        if (_outlineMaterial != null)
        {
            Destroy(_outlineMaterial);
        }
    }
}
