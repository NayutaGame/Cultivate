
using TMPro;
using UnityEngine;

public class ResultRowView : XView
{
    [SerializeField] private TMP_Text Text;
    
    public override void Refresh()
    {
        base.Refresh();
    
        ResultRow resultRow = Get<ResultRow>();
        Text.text = resultRow.GetDescription().GetHighlightedString();
        
        if (resultRow.IsActive)
            Text.color = Color.green;
        else
            Text.color = Color.black;
    }
}
