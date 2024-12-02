
using UnityEngine;

public class PinAnchor : MonoBehaviour
{
    private RectTransform _rect;
    public RectTransform GetRect() => _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
}
