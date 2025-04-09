
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

        ConfigPack pack = Get<ConfigPack>();
        SetName(pack.GetName());
        Image.sprite = pack.GetSprite();

        if (!ParentedByConstraint)
        {
            EquippedGameObject.SetActive(pack.Equipped());
            bool locked = !pack.IsUnlocked();
            LockGameObject.SetActive(locked);
            if (locked)
                UnlockConditionText.text = pack.GetUnlockCondition();
        }
    }

    public void RefreshFromParentedConstraint(PackConstraint constraint)
    {
        EquippedGameObject.SetActive(false);
        bool locked = !constraint.IsUnlocked();
        LockGameObject.SetActive(locked);
        if (locked)
            UnlockConditionText.text = constraint.GetUnlockCondition();
    }

    protected virtual void SetName(string name)
    {
        NameText.text = name;
    }
}
