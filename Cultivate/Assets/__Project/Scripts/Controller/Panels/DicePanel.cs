
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DicePanel : Panel
{
    [SerializeField] private ListView GainList;

    [SerializeField] private TMP_Text DiceDescription;
    [SerializeField] private TMP_Text DiceEquation;
    [SerializeField] private TMP_Text FinalScoreText;
    [SerializeField] private TMP_Text DiceRangeText;
    
    [SerializeField] private ListView ResultList;
    
    [SerializeField] private Button4State RollButton;
    [SerializeField] private Button4State ForwardButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        
        GainList.SetAddress(_address.Append(".GainTable"));
        ResultList.SetAddress(_address.Append(".ResultTable"));
        RollButton.LeftClickNeuron.Add(Roll);
        ForwardButton.LeftClickNeuron.Add(Forward);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        DiceCell cell = cellAdapter.AsCell() as DiceCell;

        bool isRolled = cell.State == DiceCell.DiceCellState.Rolled;
        int totalGain = cell.GetTotalGain();
        bool hasAtLeastOneGainRow = cell.HasAtLeastOneGainRow();
        
        // 按钮状态
        RollButton.SetStateToActiveIf(!isRolled);
        ForwardButton.SetStateToActiveIf(isRolled);

        DiceDescription.text = cell.DiceDescription;
        
        // 方程式文本（只有在有加分选项时才显示）
        DiceEquation.gameObject.SetActive(hasAtLeastOneGainRow);
        if (hasAtLeastOneGainRow)
        {
            if (isRolled)
            {
                // Rolled 状态：显示实际值 "15 + 3"
                int diceValue = cell.GetDiceValue();
                DiceEquation.text = totalGain != 0 
                    ? $"{diceValue} + [{totalGain}]" 
                    : diceValue.ToString();
            }
            else
            {
                // Unrolled 状态：显示 "? + 3" 或 "?"
                DiceEquation.text = totalGain != 0 
                    ? $"? + [{totalGain}]" 
                    : "?";
            }
        }
        
        // 最终值文本
        if (isRolled)
        {
            // Rolled 状态：显示最终值 "18"
            FinalScoreText.text = cell.GetFinalDiceValue().ToString();
        }
        else
        {
            // Unrolled 状态：显示 "?" 或显示可能的范围
            FinalScoreText.text = "?";
        }
        
        // 范围文本（始终显示）
        DiceRangeText.text = $"骰子范围 ~ [1, {cell.DiceRange}]";
        
        GainList.Refresh();
        ResultList.Refresh();
    }

    private void Roll(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(new RollSignal());
        
        // staging
        Refresh();
    }

    private void Forward(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(new ExitDiceSignal());
    }
}