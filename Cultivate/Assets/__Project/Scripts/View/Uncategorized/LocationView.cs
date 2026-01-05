
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationView : XView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text VisitorText;
    [SerializeField] private TMP_Text ClueText;

    public override void Refresh()
    {
        base.Refresh();

        Location location = Get<Location>();

        switch (location.State)
        {
            case LocationState.Sealed:
                BackgroundImage.color = Color.gray;
                break;
            case LocationState.Current:
                BackgroundImage.color = Color.cyan;
                break;
            case LocationState.Available:
                BackgroundImage.color = Color.white;
                break;
        }
        
        NameText.text = location.Entry.GetName();
        VisitorText.text = "无人访问";
        ClueText.text = "";
    }
}