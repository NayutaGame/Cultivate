
using System;
using System.Collections.Generic;
using CLLibrary;

public class DifficultyRunConfigTabControl : RunConfigTabControl
{
    public Neuron<DifficultySelectDetails> DifficultySelectNeuron = new();

    private DifficultyProfile _difficulty;
    private DifficultyProfile _recordedDifficulty;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Slots",                      thisObject => ((RunConfigTabControl)thisObject)._filteredSlots },
    };
    public override object Get(string s) => Accessor[s](this);
    public DifficultyRunConfigTabControl()
    {
    }

    public DifficultyProfile GetSelectedDifficultyProfile()
        => _difficulty;
    
    public int GetSelectedDifficultyIndex()
        => AppManager.Instance.ProfileManager.GetCurrProfile().DifficultyProfileList.IndexOf(_difficulty);

    public void SelectHighestUnlockedDifficulty()
    {
        DifficultyProfile difficultyProfile = AppManager.Instance.ProfileManager.GetCurrProfile().GetHighestUnlockedDifficulty();
        SelectDifficultyProcedureWithoutWrite(new DifficultySelectDetails(difficultyProfile));
    }

    public void SelectDifficultyProcedureWithoutWrite(DifficultySelectDetails d)
    {
        if (d.ToDifficulty == _difficulty)
            return;

        DifficultyProfileList difficultyProfileList = AppManager.Instance.ProfileManager.GetCurrProfile().DifficultyProfileList;
        
        d.FromIndex = difficultyProfileList.IndexOf(_difficulty);
        d.FromDifficulty = _difficulty;
        
        _difficulty = d.ToDifficulty;
        
        d.ToIndex = difficultyProfileList.IndexOf(_difficulty);
        
        DifficultySelectNeuron.Invoke(d);
    }

    public void SelectDifficultyProcedure(DifficultySelectDetails d)
    {
        if (d.ToDifficulty == _difficulty)
            return;
        
        SelectDifficultyProcedureWithoutWrite(d);
        AppManager.Instance.ConfigManager.TryWriteRecord();
    }

    public override void WriteRecord()
    {
        _recordedDifficulty = _difficulty;
    }

    public override void ReadRecord()
    {
        _difficulty = _recordedDifficulty;
    }

    public override bool IsValid()
    {
        return _difficulty.IsUnlocked();
    }
}