
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DicePanel : Panel
{
    [SerializeField] private FixedListView GainList;

    [SerializeField] private TMP_Text DialogText;
    [SerializeField] private TMP_Text RolledPointsText;
    [SerializeField] private TMP_Text GainedPointsText;
    [SerializeField] private TMP_Text DiceRangeText;
    
    [SerializeField] private FixedListView OutcomeList;
    
    [SerializeField] private CLButtonPatternA ForwardButton;
    [SerializeField] private TMP_Text ForwardButtonText;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        
        GainList.SetAddress(_address.Append(".GainTable"));
        OutcomeList.SetAddress(_address.Append(".ResultTable"));
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
        
        DialogText.text = cell.DiceDescription;

        if (!isRolled)
        {
            ForwardButtonText.text = "投掷";
            RolledPointsText.text = "";
            GainedPointsText.text = "";
        }
        else
        {
            ForwardButtonText.text = "结算";
            RolledPointsText.text = cell.GetDiceValue().ToString();
            GainedPointsText.text = totalGain != 0 ? $"+{totalGain}点" : "";
        }
        
        // 范围文本（始终显示）
        DiceRangeText.text = $"骰子范围 ~ [1, {cell.DiceRange}]";
        
        GainList.Refresh();
        OutcomeList.Refresh();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.DiceGainedPointsChangedNeuron.Join(Refresh);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.DiceGainedPointsChangedNeuron.Remove(Refresh);
    }

    private void Forward(InteractBehaviour ib, PointerEventData d)
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        DiceCell cell = cellAdapter.AsCell() as DiceCell;

        bool isRolled = cell.State == DiceCell.DiceCellState.Rolled;
        if (!isRolled)
        {
            RunManager.Instance.Environment.ReceiveSignalProcedure(new RollSignal());
            // staging
            Refresh();
        }
        else
        {
            RunManager.Instance.Environment.ReceiveSignalProcedure(new ExitDiceSignal());
        }
    }
}