
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text StackText;
    [SerializeField] private Image IconImage;

    public override void Refresh()
    {
        base.Refresh();

        Buff b = Get<Buff>();
        StackText.text = b.Stack.ToString();
        IconImage.sprite = b.GetEntry().GetSprite();

        NameText.text = IconImage.sprite == Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite ? b.GetName() : "";
    }
}
