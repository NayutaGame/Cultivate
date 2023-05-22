
using CLLibrary;
using TMPro;
using UnityEngine.Serialization;

public class CharacterPanel : Panel
{
    public CharacterPanelState _state;

    public StatusView StatusView;
    public TMP_Text MingYuanText;

    public override void Configure()
    {
        StatusView.Configure();
        _state = new CharacterPanelStateNormal();
    }

    public override void Refresh()
    {
        StatusView.Refresh();

        MingYuanText.text = $"命元：{RunManager.Instance.MingYuan}";
    }
}
