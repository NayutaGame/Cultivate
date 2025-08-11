
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackView : XView
{
    [SerializeField] private bool ParentedByConstraint;
    
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private Image Image;
    [SerializeField] private GameObject EquippedGameObject;
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

    public void RefreshFromParentedConstraint(PackConstraint constraint)
    {
        EquippedGameObject.SetActive(false);
        bool locked = !constraint.IsUnlocked();
        LockGameObject.SetActive(locked);
        if (locked)
            UnlockConditionText.text = constraint.GetUnlockCondition().GetHighlightedString();
    }

    private bool InterpretAsConfigPack(object obj)
    {
        ConfigPack pack = obj as ConfigPack;
        if (pack == null)
            return false;
        
        NameText.text = pack.GetName();
        Image.sprite = pack.GetSprite();

        if (!ParentedByConstraint)
        {
            EquippedGameObject.SetActive(pack.Equipped());
            bool locked = !pack.IsUnlocked();
            LockGameObject.SetActive(locked);
            if (locked)
                UnlockConditionText.text = pack.GetUnlockCondition().GetHighlightedString();
        }

        return true;
    }

    private bool InterpretAsPackConstraint(object obj)
    {
        PackConstraint pack = obj as PackConstraint;
        if (pack == null)
            return false;
        
        NameText.text = pack.GetName();
        Image.sprite = pack.GetSprite();

        if (!ParentedByConstraint)
        {
            EquippedGameObject.SetActive(false);
            bool locked = !pack.IsUnlocked();
            LockGameObject.SetActive(locked);
            if (locked)
                UnlockConditionText.text = pack.GetUnlockCondition().GetHighlightedString();
        }

        return true;
    }

    private bool InterpretAsPackEntry(object obj)
    {
        PackEntry pack = obj as PackEntry;
        if (pack == null)
            return false;

        NameText.text = pack.GetName();
        Image.sprite = pack.GetSprite();
        
        EquippedGameObject.SetActive(false);
        LockGameObject.SetActive(false);

        return true;
    }
}
