
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MarkView : XView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text NumberText;

    public override void Refresh()
    {
        base.Refresh();

        MarkModel mark = Get<MarkModel>();
        JingJieText.text = $"{mark._tag}";
        NumberText.text = $"{mark._mark}";
    }

    public void SetState(bool activated, bool showing)
    {
        if (activated)
        {
            BackgroundImage.color = new Color(1, 1, 1, 0.7f);
            return;
        }

        if (showing)
        {
            BackgroundImage.color = new Color(1, 1, 1, 0.5f);
            return;
        }

        BackgroundImage.color = new Color(1, 1, 1, 0.2f);
    }
}
