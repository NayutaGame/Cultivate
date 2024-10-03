
using TMPro;

public class EntityBarView : SimpleView
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;

    public override void Refresh()
    {
        base.Refresh();

        IEntity entity = Get<IEntity>();

        NameText.text = entity.GetEntry()?.GetName() ?? "未命名";
        JingJieText.text = entity.GetJingJie().ToString();
    }
}
