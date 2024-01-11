
using TMPro;

public class EntityView : SimpleView
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

    // private InteractHandler _interactHandler;
    // public InteractHandler GetHandler() => _interactHandler;
    // public void SetHandler(InteractHandler interactHandler)
    // {
    //     _interactHandler = interactHandler;
    //     if (SlotListView != null)
    //         SlotListView.SetHandler(_interactHandler);
    // }
}
