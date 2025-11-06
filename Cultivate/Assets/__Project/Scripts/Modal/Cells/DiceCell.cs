
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DiceCell : Cell
{
    public enum DiceCellState
    {
        Unrolled,
        Rolled,
    }
    
    public int DiceRange { get; private set; }
    public ListModel<GainRow> GainTable { get; private set; }
    public ListModel<ResultRow> ResultTable { get; private set; }
    public string DiceDescription { get; private set; }

    public DiceCellState State { get; private set; }
    public int DiceValue { get; private set; }
    public Dirty<int> ResultIndex { get; private set; }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Guide",                      thisObject => ((DialogCell)thisObject).GetGuideDescriptor() },
        { "GainTable",                  thisObject => ((DiceCell)thisObject).GainTable },
        { "ResultTable",                thisObject => ((DiceCell)thisObject).ResultTable },
    };
    public override object Get(string s) => Accessor[s](this);
    public DiceCell(int diceRange, GainRow[] gainTable, ResultRow[] resultTable, string diceDescription)
    {
        DiceRange = diceRange;
        GainTable = gainTable != null ? new(gainTable) : new();
        ResultTable = resultTable != null ? new(resultTable) : new();
        DiceDescription = diceDescription;

        State = DiceCellState.Unrolled;
        DiceValue = -1;
        ResultIndex = new Dirty<int>(CalcResultIndex);
    }

    public bool HasAtLeastOneGainRow()
    {
        return GainTable.Count() > 0;
    }

    public int GetDiceValue()
    {
        return DiceValue;
    }

    public int GetTotalGain()
    {
        int totalGain = 0;
        GainTable.Do(r => totalGain += r.CalculateGain());
        return totalGain;
    }

    public int GetFinalDiceValue()
    {
        return GetDiceValue() + GetTotalGain();
    }

    public int GetResultIndex()
    {
        return ResultIndex.Value;
    }

    public void Roll()
    {
        DiceValue = UnityEngine.Random.Range(1, DiceRange + 1);
        State = DiceCellState.Rolled;
        InvalidateCache();
    }

    private int CalcResultIndex()
    {
        // 如果未投掷，返回 -1
        if (State != DiceCellState.Rolled || DiceValue < 0)
        {
            ResultTable.Do(r => r.IsActive = false);
            return -1;
        }

        int finalDiceValue = GetFinalDiceValue();
        int resultIndex = -1;
        
        // 如果 ResultTable 为空，返回 -1
        if (ResultTable.Count() == 0)
        {
            return -1;
        }
        
        // 重置所有 ResultRow 的 IsActive 状态
        ResultTable.Do(r => r.IsActive = false);
        
        // 检查极端情况：小于第一个 ResultRow 的最小值，返回第一个
        if (finalDiceValue < ResultTable[0].GetMinValue())
        {
            resultIndex = 0;
            ResultTable[0].IsActive = true;
            return resultIndex;
        }
        
        // 检查极端情况：大于最后一个 ResultRow 的最大值，返回最后一个
        int lastIndex = ResultTable.Count() - 1;
        if (finalDiceValue > ResultTable[lastIndex].GetMaxValue())
        {
            resultIndex = lastIndex;
            ResultTable[lastIndex].IsActive = true;
            return resultIndex;
        }
        
        // 查找匹配的 ResultRow
        for (int i = 0; i < ResultTable.Count(); i++)
        {
            if (ResultTable[i].IsInRange(finalDiceValue))
            {
                resultIndex = i;
                ResultTable[i].IsActive = true;
                break; // 找到第一个匹配的就退出
            }
        }
        return resultIndex;
    }

    private void InvalidateCache()
    {
        GainTable.Do(r => r.InvalidateCache());
        ResultIndex.SetDirty();
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is RollSignal rollSignal)
        {
            Roll();
            return this;
        }

        if (signal is ExitDiceSignal exitDiceSignal)
        {
            return null;
        }

        return this;
    }
}