
using TMPro;
using UnityEngine;

public class DetailedCharacterProfileView : LegacySimpleView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text AbilityDescriptionText;
    [SerializeField] private TMP_Text UnlockConditionText;

    public override void Refresh()
    {
        base.Refresh();

        if (GetAddress() == null)
        {
            NameText.text = "请选择一名角色";
            AbilityDescriptionText.text = "角色的能力";
            UnlockConditionText.gameObject.SetActive(false);
            return;
        }

        CharacterProfile p = Get<CharacterProfile>();
        NameText.text = p.GetEntry().GetName();
        AbilityDescriptionText.text = p.GetEntry().AbilityDescription;

        UnlockConditionText.gameObject.SetActive(!p.IsUnlocked());
        UnlockConditionText.text = p.GetEntry().UnlockConditionDescription;
    }
}
