
public class AnimatedItemView : ItemView
{
    public ItemView ItemView;
    public PivotDelegate PivotDelegate { get; set; }
    public InteractDelegate InteractDelegate;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        ItemView.SetAddress(address);
    }

    public override void Refresh()
    {
        base.Refresh();
        ItemView.Refresh();
    }

    private void OnEnable()
    {
        PivotDelegate.gameObject.SetActive(true);
        InteractDelegate.PlayFollowAnimation(ItemView.RectTransform, PivotDelegate.IdlePivot);

        // SkillView.CanvasGroup.blocksRaycasts = true;
    }

    private void OnDisable()
    {
        PivotDelegate.gameObject.SetActive(false);
    }
}
