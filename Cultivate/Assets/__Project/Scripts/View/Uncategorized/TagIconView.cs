
using UnityEngine;
using UnityEngine.UI;

public class TagIconView : XView
{
    [SerializeField] private Image IconImage;

    public override void Refresh()
    {
        base.Refresh();

        IconImage.sprite = Get<TagEntry>().GetIcon();
    }
}
