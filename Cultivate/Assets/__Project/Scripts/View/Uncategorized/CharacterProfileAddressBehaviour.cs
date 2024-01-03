
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileAddressBehaviour : AddressBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile p = Get<CharacterProfile>();
        Image.color = p.IsUnlocked() ? Color.white : Color.gray;
        NameText.text = p.GetEntry().Name;
    }
}
