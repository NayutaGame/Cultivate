
using TMPro;
using UnityEngine;

public class MapNodeView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text VisitorText;
    [SerializeField] private TMP_Text ClueText;

    public override void Refresh()
    {
        base.Refresh();

        MapNode mapNode = Get<MapNode>();
        NameText.text = mapNode.Entry.GetName();
        VisitorText.text = "无人访问";
        ClueText.text = "";
    }
}