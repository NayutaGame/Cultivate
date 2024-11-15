
using UnityEngine;

[RequireComponent(typeof(XView))]
public abstract class XBehaviour : MonoBehaviour
{
    protected XView XView;

    public virtual void Init(XView xView)
    {
        XView = xView;
    }
}
