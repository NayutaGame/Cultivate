
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeView : XView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text VisitorText;
    [SerializeField] private TMP_Text ClueText;

    public override void Refresh()
    {
        base.Refresh();

        MapNode mapNode = Get<MapNode>();
        
        if (mapNode.IsAccessible)
            BackgroundImage.color = Color.white;
        else
            BackgroundImage.color = Color.gray;
        
        NameText.text = mapNode.Entry.GetName();
        VisitorText.text = "无人访问";
        ClueText.text = "";
    }
}