
using UnityEngine;

[RequireComponent(typeof(XView))]
public abstract class XBehaviour : MonoBehaviour
{
    protected XView View;

    public virtual void Init(XView view)
    {
        View = view;
    }
}
