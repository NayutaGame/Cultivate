
using TMPro;
using UnityEngine;

public class GainRowView : XView
{
    [SerializeField] private TMP_Text Text;
    
    public override void Refresh()
    {
        base.Refresh();
    
        GainRow gainRow = Get<GainRow>();
        Text.text = gainRow.GetDescription().GetHighlightedString();
        GainRow.GainStyle gainStyle = gainRow.GetGainStyle();
        switch (gainStyle)
        {
            case GainRow.GainStyle.Positive:
                Text.color = new Color(0.16f, 0.8f, 0.16f);
                break;
            case GainRow.GainStyle.Negative:
                Text.color = Color.red;
                break;
            case GainRow.GainStyle.Inactive:
                Text.color = Color.gray;
                break;
        }
    }
}