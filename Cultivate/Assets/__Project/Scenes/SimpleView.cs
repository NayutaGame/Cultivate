
using TMPro;
using UnityEngine;

public class SimpleView : XView
{
    [SerializeField] private TMP_Text NameText;
    
    public override void Refresh()
    {
        base.Refresh();

        SimpleItem simpleItem = Get<SimpleItem>();

        NameText.text = simpleItem.Name;
    }
}
