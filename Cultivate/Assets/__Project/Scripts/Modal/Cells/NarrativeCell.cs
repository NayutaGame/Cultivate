
using System;
using System.Collections.Generic;

public class NarrativeCell : Cell
{
    private int _commendIndex;
    private List<Commend> _commends;

    private CharacterEntry _leftCharacter;
    private CharacterEntry _rightCharacter;
    private string _narrativeText;

    public CharacterEntry GetLeftCharacter() => _leftCharacter;
    public CharacterEntry GetRightCharacter() => _rightCharacter;
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

    public void SetCharacter(CharacterEntry characterEntry, bool isHome)
    {
        if (isHome)
            _leftCharacter = characterEntry;
        else
            _rightCharacter = characterEntry;

        RunManager.Instance.Environment.CharacterChangedNeuron.Invoke(characterEntry, isHome);
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