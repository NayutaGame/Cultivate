
using CLLibrary;
using UnityEngine;

public class PinAnchor : Singleton<PinAnchor>
{
    private RectTransform _rect;
    public RectTransform GetRect() => _rect;
    
    public override void DidAwake()
    {
        base.DidAwake();
        _rect = GetComponent<RectTransform>();
    }
}
