
using TMPro;

public class EntityView : ItemView, IInteractable
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public ListView SlotListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        if (SlotListView != null)
            SlotListView.SetAddress(GetAddress().Append($"Slots"));
    }

    public override void Refresh()
    {
        base.Refresh();
        IEntityModel entity = Get<IEntityModel>();
        if (NameText != null)
            NameText.text = entity.GetEntry()?.Name ?? "未命名";
        if (JingJieText != null)
            JingJieText.text = entity.GetJingJie().ToString();
        if (HPText != null)
            HPText.text = entity.GetFinalHealth().ToString();
        if (DescriptionText != null)
            DescriptionText.text = entity.GetEntry()?.Description ?? "这家伙很懒，什么都没有写";
        if (SlotListView != null)
            SlotListView.Refresh();
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        if (SlotListView != null)
            SlotListView.SetDelegate(InteractDelegate);
    }

    #endregion
}
