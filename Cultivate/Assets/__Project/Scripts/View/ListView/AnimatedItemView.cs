
public class AnimatedItemView : ItemView
{
    public InteractBehaviour InteractBehaviour;

    private void OnEnable()
    {
        InteractBehaviour.SetEnabled(true);
    }

    private void OnDisable()
    {
        InteractBehaviour.SetEnabled(false);
    }
}
