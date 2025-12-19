
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackView : XView
{
    [SerializeField] private Image PackImage;
    [SerializeField] private Image EquippedImage;
    [SerializeField] private GameObject LockGameObject;
    [SerializeField] private TMP_Text UnlockConditionText;

    public override void Refresh()
    {
        base.Refresh();

        object obj = Get<object>();
        if (InterpretAsConfigPack(obj)) return;
        if (InterpretAsPackConstraint(obj)) return;
        if (InterpretAsPackEntry(obj)) return;
    }

    public bool InterpretAsConfigPack(object obj)
    {
        ConfigPack pack = obj as ConfigPack;
        if (pack == null)
            return false;
        
        PackImage.sprite = pack.GetSprite();
        EquippedImage.gameObject.SetActive(pack.Equipped());
        bool locked = !pack.IsUnlocked();
        LockGameObject.SetActive(locked);
        if (locked)
            UnlockConditionText.text = $"<rotate=90>{pack.GetUnlockCondition().GetHighlightedString()}</rotate>";

        return true;
    }

    public bool InterpretAsPackConstraint(object obj)
    {
        PackConstraint pack = obj as PackConstraint;
        if (pack == null)
            return false;
        
        PackImage.sprite = pack.GetSprite();
        EquippedImage.gameObject.SetActive(false);
        bool locked = !pack.IsUnlocked();
        LockGameObject.SetActive(locked);
        if (locked)
            UnlockConditionText.text = $"<rotate=90>{pack.GetUnlockCondition().GetHighlightedString()}</rotate>";

        return true;
    }

    public bool InterpretAsPackEntry(object obj)
    {
        PackEntry pack = obj as PackEntry;
        if (pack == null)
            return false;

        PackImage.sprite = pack.GetSprite();
        EquippedImage.gameObject.SetActive(false);
        LockGameObject.SetActive(false);

        return true;
    }
}
