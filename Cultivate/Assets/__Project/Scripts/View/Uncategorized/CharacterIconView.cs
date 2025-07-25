
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconView : XView
{
    [SerializeField] private Image Icon;

    public override void Refresh()
    {
        base.Refresh();

        object obj = Get<object>();

        if (InterpretAsCharacterProfile(obj)) return;
        if (InterpretAsCharacterEntry(obj)) return;
    }

    private bool InterpretAsCharacterProfile(object obj)
    {
        CharacterProfile character = obj as CharacterProfile;
        if (character == null)
            return false;

        Icon.sprite = character.GetEntry().GetCharacterIconSprite();
        
        return true;
    }

    private bool InterpretAsCharacterEntry(object obj)
    {
        CharacterEntry character = obj as CharacterEntry;
        if (character == null)
            return false;

        Icon.sprite = character.GetCharacterIconSprite();
        
        return true;
    }
}
