
using UnityEngine;

[RequireComponent(typeof(CLView))]
public abstract class ExtraBehaviour : MonoBehaviour
{
    protected CLView CLView;

    public virtual void Init(CLView clView)
    {
        CLView = clView;
    }
}
