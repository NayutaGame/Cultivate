
using DG.Tweening;
using UnityEngine;

public abstract class CLAnimation
{
    protected RectTransform Content;
    protected Configuration StartConfiguration;
    
    public CLAnimation(RectTransform content)
    {
        Content = content;
    }
    
    public abstract Tween GetHandle();

    public void AppendHandle(Sequence seq)
    {
        seq.Append(GetHandle());
    }
    
    protected abstract void SetProgress(float t);

    public void RecordConfiguration()
    {
        StartConfiguration = Configuration.FromRect(Content);
    }
}