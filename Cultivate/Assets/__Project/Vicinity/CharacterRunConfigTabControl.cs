
using System;
using System.Collections.Generic;
using CLLibrary;

public class CharacterRunConfigTabControl : RunConfigTabControl
{
    public Neuron<CharacterSelectDetails> CharacterSelectNeuron = new();

    private CharacterProfile _character;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Character",                      thisObject => ((CharacterRunConfigTabControl)thisObject)._filteredSlots },
    };
    public object Get(string s) => Accessor[s](this);
    public CharacterRunConfigTabControl()
    {
    }

    public CharacterProfile GetSelectedCharacterProfile()
        => _character;
    
    public int GetSelectedCharacterIndex()
        => AppManager.Instance.ProfileManager.GetCurrProfile().CharacterProfileList.IndexOf(_character);

    public void SelectFirstCharacter()
    {
        SelectCharacterProcedure(new CharacterSelectDetails(AppManager.Instance.ProfileManager.GetCurrProfile().FirstCharacterProfile()));
    }

    public void SelectCharacterProcedure(CharacterSelectDetails d)
    {
        if (d.ToCharacter == _character)
            return;

        CharacterProfileList characterProfileList = AppManager.Instance.ProfileManager.GetCurrProfile().CharacterProfileList;
        
        d.FromIndex = characterProfileList.IndexOf(_character);
        d.FromCharacter = _character;
        
        _character = d.ToCharacter;
        
        d.ToIndex = characterProfileList.IndexOf(_character);
        
        CharacterSelectNeuron.Invoke(d);

        // LoadPackPresetFromCharacter(_character);
    }

    // private void LoadPackPresetFromCharacter(CharacterProfile character)
    // {
    //     PackPreset preset = character.GetEntry().PackPreset;
    //     LoadPackPreset(preset);
    // }
    //
    // public PackPreset WriteCurrentIntoPackPreset()
    // {
    //     List<PackEntry> packEntries = new();
    //     _packConstraints.Do(c => packEntries.Add(c.Pack.Entry));
    //     return new PackPreset(packEntries);
    // }
    //
    // public void LoadPackPreset(PackPreset preset)
    // {
    //     _packConstraints.Do(c => c.Pack = null);
    //     _packSelections.Do(p => p.IsEquipped = false);
    //
    //     for(int i = 0; i < preset.PackEntries.Count; i++)
    //     {
    //         PackEntry pack = preset.PackEntries[i];
    //         ConfigPack configPack = _packSelections.First(p => p.Entry == pack);
    //         configPack.IsEquipped = true;
    //
    //         _packConstraints[i].Pack = configPack;
    //     }
    // }
}