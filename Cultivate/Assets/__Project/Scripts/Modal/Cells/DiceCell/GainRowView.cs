
using TMPro;
using UnityEngine;

public class GainRowView : XView
{
    public static Color PositiveColor = new Color(0.4470f, 0.8235f, 0.6666f, 1f);
    public static Color ZeroColor = new Color(0.6549f, 0.6823f, 0.7607f, 1f);
    public static Color NegativeColor = new Color(0.8156f, 0.3607f, 0.3607f, 1f);
    
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private TMP_Text ScoreText;
    
    public override void Refresh()
    {
        base.Refresh();
    
        GainRow gainRow = Get<GainRow>();
        DescriptionText.text = gainRow.GetDescriptionText().GetHighlightedString();
        ScoreText.text = gainRow.GetScoreText();
        GainRow.GainStyle gainStyle = gainRow.GetGainStyle();
        switch (gainStyle)
        {
            case GainRow.GainStyle.Positive:
                ScoreText.color = PositiveColor;
                break;
            case GainRow.GainStyle.Negative:
                ScoreText.color = NegativeColor;
                break;
            case GainRow.GainStyle.Inactive:
                ScoreText.color = ZeroColor;
                break;
        }
    }
}