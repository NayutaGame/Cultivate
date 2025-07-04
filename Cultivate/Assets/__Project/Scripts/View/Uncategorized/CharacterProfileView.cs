
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileView : XView
{
    [NonSerialized] private SelectBehaviour _selectBehaviour;
    [SerializeField] private Image CharacterIcon;
    [SerializeField] private Image IsDemoLockedIcon;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _selectBehaviour = GetBehaviour<SelectBehaviour>();
        
        GetInteractBehaviour().PointerEnterNeuron.Join(AudioManager.PlayButtonHover);
        GetInteractBehaviour().LeftClickNeuron.Join(AudioManager.PlayButtonPress);
    }

    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile p = Get<CharacterProfile>();
        CharacterIcon.sprite = p.GetEntry().GetCharacterIconSprite();
        _selectBehaviour.SetSelectionSprite(p.GetEntry().GetCharacterIconSelectSprite());
        CharacterIcon.color = p.IsUnlocked() ? Color.white : Color.gray;
        
        IsDemoLockedIcon.gameObject.SetActive(p.IsDemoLocked());
    }
}
