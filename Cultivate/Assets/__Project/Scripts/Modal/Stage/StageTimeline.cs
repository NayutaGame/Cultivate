using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class StageTimeline : Addressable
{
    private List<StageNote> _notes;
    private int _pointer;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Notes",                      thisObject => ((StageTimeline)thisObject)._notes },
    };
    public object Get(string s) => Accessor[s](this);
    public StageTimeline()
    {
        _notes = new List<StageNote>();
        _pointer = -1;
    }

    public List<StageNote> GetNotes(int start, int length)
    {
        List<StageNote> batch = new List<StageNote>();

        for (int i = 0; i < length; i++)
        {
            int shifted = i + start;
            if (0 <= shifted && shifted < _notes.Count)
            {
                batch.Add(_notes[shifted]);
            }
        }

        return batch;
    }

    public StageNote GetNote(int index)
    {
        if (0 <= index && index < _notes.Count)
        {
            return _notes[index];
        }

        return null;
    }

    public void AppendNote(int entityIndex, StageSkill skill, CostDescription actualCostDescription, string actualDescription)
    {
        int count = _notes.Count;
        StageNote stageNote = new StageNote(entityIndex, count, skill);
        stageNote.ActualCostDescription = actualCostDescription;
        stageNote.ActualDescription = actualDescription;
        _notes.Add(stageNote);
    }

    public void AppendChannelNote(int entityIndex, StageSkill skill, int currCounter, int maxCounter)
    {
        int count = _notes.Count;
        _notes.Add(new StageNote(entityIndex, count, skill, currCounter, maxCounter));
    }
}
