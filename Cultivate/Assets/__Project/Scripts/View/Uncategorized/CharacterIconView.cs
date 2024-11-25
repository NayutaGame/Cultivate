
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconView : LegacySimpleView
{
    [SerializeField] private Image Icon;
    
    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile characterProfile = Get<CharacterProfile>();
    }
}
