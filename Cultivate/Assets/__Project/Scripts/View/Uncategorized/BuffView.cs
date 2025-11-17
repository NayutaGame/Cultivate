
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
        
        object obj = Get<object>();
        if (TryInterpretAsBuff(obj as Buff)) return;
        if (TryInterpretAsBuffEntry(obj as BuffEntry)) return;
    }

    private bool TryInterpretAsBuff(Buff b)
    {
        if (b is null)
            return false;

        StackText.text = b.Stack.ToString();
        IconImage.sprite = b.GetEntry().GetSprite();

        NameText.text = IconImage.sprite == Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite ? b.GetName() : "";

        return true;
    }

    private bool TryInterpretAsBuffEntry(BuffEntry b)
    {
        if (b is null)
            return false;

        StackText.text = "";
        IconImage.sprite = b.GetSprite();

        NameText.text = IconImage.sprite == Encyclopedia.SpriteCategory.MissingBuffIcon().Sprite ? b.GetName() : "";

        return true;
    }
}
