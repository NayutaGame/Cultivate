
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ConfigManager : Addressable
{
    public Neuron<RunConfigTabChangedDetails> TabChangedNeuron = new();
    
    private ListModel<RunConfigTabControl> _tabs;
    private RunConfigTabControl _selectedTab;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "RunConfigTabControls",       thisObject => ((ConfigManager)thisObject)._tabs },
        { "CharacterTabControl",        thisObject => ((ConfigManager)thisObject).CharacterTabControl },
        { "DifficultyTabControl",       thisObject => ((ConfigManager)thisObject).DifficultyTabControl },
        { "PackTabControl",             thisObject => ((ConfigManager)thisObject).PackTabControl },
    };
    public object Get(string s) => Accessor[s](this);
    public ConfigManager()
    {
        _tabs = new ListModel<RunConfigTabControl>();
        _tabs.Add(new CharacterRunConfigTabControl());
        _tabs.Add(new DifficultyRunConfigTabControl());
        _tabs.Add(new PackRunConfigTabControl());
    }

    public RunConfigTabControl GetSelectedTab()
        => _selectedTab;
    
    public int GetSelectedIndex()
        => _tabs.IndexOf(_selectedTab);

    public CharacterRunConfigTabControl CharacterTabControl
        => _tabs[0] as CharacterRunConfigTabControl;
    
    public DifficultyRunConfigTabControl DifficultyTabControl
        => _tabs[1] as DifficultyRunConfigTabControl;
    
    public PackRunConfigTabControl PackTabControl
        => _tabs[2] as PackRunConfigTabControl;
    
    public void ResetProcedure()
    {
        SelectTabProcedure(_tabs[0]);
        
        CharacterTabControl.SelectFirstCharacter();
        DifficultyTabControl.SelectHighestUnlockedDifficulty();
        TryWriteRecord();
    }

    public void SelectTabProcedure(RunConfigTabControl tab)
    {
        if (tab == _selectedTab)
            return;
        
        int fromIndex = _tabs.IndexOf(_selectedTab);
        _selectedTab = tab;
        int toIndex = _tabs.IndexOf(_selectedTab);
        TabChangedNeuron.Invoke(new(fromIndex, toIndex));
    }

    public void ProcessProcedure()
    {
        if (!IsValid())
            return;

        int currentTabIndex = GetSelectedIndex();
        if (currentTabIndex < _tabs.Count() - 1)
        {
            SelectTabProcedure(_tabs[currentTabIndex + 1]);
        }
        else
        {
            StartRunProcedure();
        }
    }
    
    private void StartRunProcedure()
    {
        CharacterProfile characterProfile = CharacterTabControl.GetSelectedCharacterProfile();
        DifficultyProfile difficultyProfile = DifficultyTabControl.GetSelectedDifficultyProfile();
        List<PackEntry> packEntries = PackTabControl.GetEquippedPacks();
        RunConfig runConfig = new(characterProfile, difficultyProfile, packEntries);
        AppManager.Instance.Push(AppStateMachine.RUN, runConfig);
    }

    public void TryWriteRecord()
    {
        if (IsValid())
            _tabs.Do(tab => tab.WriteRecord());
    }

    public void ReadRecord()
    {
        _tabs.Do(tab => tab.ReadRecord());
    }
    
    public bool IsValid()
    {
        RunConfigTabControl firstInvalid = _tabs.FirstObj(tab => !tab.IsValid());
        return firstInvalid == null;
    }
}
