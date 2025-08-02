
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class CharacterProfileList : ListModel<CharacterProfile>
{
    private CharacterProfileList(bool isDeveloper = false)
    {
        Encyclopedia.CharacterCategory.Do(entry => Add(
            new CharacterProfile(entry)));

        // Find("徐福").SetUnlocked(true);
    }

    public CharacterProfile Find(CharacterEntry entry)
        => First(characterProfile => characterProfile.GetEntry() == entry);

    public static CharacterProfileList Default()
        => new();

    public void UnlockEverything()
    {
        this.Do(characterProfile =>
        {
            characterProfile._level = 10;
            characterProfile._experience = 1000;
        });
    }
}
