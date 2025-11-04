
using System;
using System.Collections.Generic;

public class NarrativeCell : Cell
{
    private int _commendIndex;
    private List<Commend> _commends;

    private string _leftCharacterName;
    private string _rightCharacterName;
    private string _narrativeText;

    public string GetLeftCharacterName() => _leftCharacterName;
    public string GetRightCharacterName() => _rightCharacterName;
    public string GetNarrativeText() => _narrativeText;
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Guide",                      thisObject => ((DialogCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public NarrativeCell(List<Commend> commands)
    {
        _commendIndex = 0;
        _commends = commands;
    }

    public bool HasNextCommend() => _commendIndex < _commends.Count;

    public void ProcessCommend()
    {
        if (!HasNextCommend())
        {
            throw new Exception("unexpected pathway");
        }

        Commend curr = _commends[_commendIndex];
        curr.Execute();
        RunManager.Instance.Environment.CommendProcessedNeuron.Invoke(curr);

        _commendIndex++;
    }

    public void SetCharacterName(string characterName, bool isHome)
    {
        if (isHome)
            _leftCharacterName = characterName;
        else
            _rightCharacterName = characterName;

        RunManager.Instance.Environment.CharacterNameChangedNeuron.Invoke(characterName, isHome);
    }

    public void SetNarrativeText(string narrativeText)
    {
        _narrativeText = narrativeText;
        RunManager.Instance.Environment.NarrativeTextChangedNeuron.Invoke(narrativeText);
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is ProcessNarrativeSignal processNarrativeSignal)
        {
            if (!HasNextCommend())
            {
                return null;
            }
            else
            {
                ProcessCommend();
                return this;
            }
        }

        return this;
    }
}