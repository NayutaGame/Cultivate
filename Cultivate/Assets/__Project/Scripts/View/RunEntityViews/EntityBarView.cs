
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
        NameText.text = entity.GetEntry()?.GetName() ?? "未命名";
        JingJieText.text = entity.GetJingJie().ToString();
    }
}
