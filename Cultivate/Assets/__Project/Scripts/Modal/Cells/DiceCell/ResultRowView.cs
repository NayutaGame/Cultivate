
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultRowView : XView
{
    public static Color ActiveColor = new Color(1, 1, 1, 1);
    public static Color InactiveColor = new Color(0.6549f, 0.6823f, 0.7607f, 1);
    [SerializeField] private Image[] FocusImage;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text OutcomeText;
    
    public override void Refresh()
    {
        base.Refresh();
    
        ResultRow resultRow = Get<ResultRow>();
        ScoreText.text = resultRow.GetScoreText();
        OutcomeText.text = resultRow.GetOutcomeText().GetHighlightedString();

        if (resultRow.IsActive)
        {
            ScoreText.color = ActiveColor;
            OutcomeText.color = ActiveColor;
            foreach (Image image in FocusImage)
                image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            ScoreText.color = InactiveColor;
            OutcomeText.color = InactiveColor;
            foreach (Image image in FocusImage)
                image.color = new Color(1, 1, 1, 0);
        }
    }
}
