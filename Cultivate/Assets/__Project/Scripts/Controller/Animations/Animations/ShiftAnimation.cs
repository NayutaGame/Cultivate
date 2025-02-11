
public class ShiftAnimation : Animation
{
    public ShiftAnimation() : base(true, false)
    {
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, TimelineView.Instance.ShiftAnimation());
    }
    
    public override bool InvolvesCharacterAnimation() => false;
}
