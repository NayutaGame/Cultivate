
using System;
using System.Collections.Generic;
using CLLibrary;

public class CharacterRunConfigTabControl : RunConfigTabControl
{
    public Neuron<CharacterSelectDetails> CharacterSelectNeuron = new();

    private CharacterProfile _character;
    private CharacterProfile _recordedCharacter;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Character",                      thisObject => ((CharacterRunConfigTabControl)thisObject)._filteredSlots },
    };
    public override object Get(string s) => Accessor[s](this);
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
        
        AppManager.Instance.ConfigManager.PackTabControl.SelectDefaultPackPresetFromCharacter(_character);
    }

    public override void WriteRecord()
    {
        _recordedCharacter = _character;
    }

    public override void ReadRecord()
    {
        _character = _recordedCharacter;
    }

    public override bool IsValid()
    {
        // 检查character是否已解锁
        return true;
    }
}