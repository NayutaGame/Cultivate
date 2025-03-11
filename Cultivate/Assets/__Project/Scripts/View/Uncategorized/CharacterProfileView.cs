
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileView : XView
{
    [SerializeField] private Image CharacterIcon;

    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile p = Get<CharacterProfile>();
        // CharacterIcon.sprite = p.GetEntry().GetSprite().Sprite;
        CharacterIcon.color = p.IsUnlocked() ? Color.white : Color.gray;
    }
}
