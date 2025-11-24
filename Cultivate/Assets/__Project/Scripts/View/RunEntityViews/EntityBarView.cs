
using TMPro;

public class EntityBarView : XView
{
    public TMP_Text LadderText;
    public TMP_Text NameText;
    public TMP_Text JingJieText;

    public override void Refresh()
    {
        base.Refresh();

        IEntity entity = Get<IEntity>();

        LadderText.text = entity.GetLadder().ToString();
        NameText.text = entity.GetModel()?.GetName() ?? "未命名";
        JingJieText.text = entity.GetJingJie().GetName();
    }
}
