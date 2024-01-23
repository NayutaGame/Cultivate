
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarkView : SimpleView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private TMP_Text TagText;

    public override void Refresh()
    {
        base.Refresh();

        MarkModel mark = Get<MarkModel>();
        TagText.text = $"{mark._tag}{mark._mark}";
    }

    public void SetState(bool activated, bool showing)
    {
        if (activated)
        {
            BackgroundImage.color = Color.green;
            return;
        }

        if (showing)
        {
            BackgroundImage.color = Color.yellow;
            return;
        }

        BackgroundImage.color = Color.red;
    }
}
