
using TMPro;

public class EntityView : LegacySimpleView
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public LegacyListView SlotListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        if (SlotListView != null)
            SlotListView.SetAddress(GetAddress().Append($"Slots"));
    }

    public override void Refresh()
    {
        base.Refresh();
        IEntity entity = Get<IEntity>();
        if (NameText != null)
            NameText.text = entity.GetEntry()?.GetName() ?? "未命名";
        if (JingJieText != null)
            JingJieText.text = entity.GetJingJie().ToString();
        if (HPText != null)
            HPText.text = entity.GetHealth().ToString();
        if (DescriptionText != null)
            DescriptionText.text = entity.GetEntry()?.GetDescription() ?? "这家伙很懒，什么都没有写";
        if (SlotListView != null)
            SlotListView.Refresh();
    }
}
