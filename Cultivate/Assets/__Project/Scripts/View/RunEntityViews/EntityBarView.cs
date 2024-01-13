
using TMPro;

public class EntityBarView : SimpleView
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;

    public override void Refresh()
    {
        base.Refresh();

        IEntityModel entity = Get<IEntityModel>();

        NameText.text = entity.GetEntry()?.Name ?? "未命名";
        JingJieText.text = entity.GetJingJie().ToString();
    }
}
