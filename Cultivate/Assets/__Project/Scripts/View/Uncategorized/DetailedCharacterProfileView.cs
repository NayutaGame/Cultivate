
using TMPro;
using UnityEngine;

public class DetailedCharacterProfileView : LegacyAddressBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text AbilityDescriptionText;

    public override void Refresh()
    {
        base.Refresh();

        if (GetAddress() == null)
        {
            NameText.text = "请选择一名角色";
            AbilityDescriptionText.text = "角色的能力";
            return;
        }

        CharacterProfile p = Get<CharacterProfile>();
        NameText.text = p.GetEntry().Name;
        AbilityDescriptionText.text = p.GetEntry().AbilityDescription;
    }
}
