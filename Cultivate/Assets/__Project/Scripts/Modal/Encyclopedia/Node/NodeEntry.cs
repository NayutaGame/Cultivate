using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeEntry : Entry
{
    private string _description;
    public string Description;

    public NodeEntry(string name, string description) : base(name)
    {
        _description = description;
    }

    public void Generator(RunNode runNode)
    {
        DialogPanelDescriptor A = new DialogPanelDescriptor();
        DialogPanelDescriptor B = new DialogPanelDescriptor();
        DialogPanelDescriptor C = new DialogPanelDescriptor();

        A.ReceiveSignal = (signal) =>
        {
            SelectedOptionSignal s = signal as SelectedOptionSignal;
            if (s == null)
                return;

            int i = s._selected;

            if (i == 0)
            {
                runNode.ChangePanel(B);
            }
            else
            {
                runNode.ChangePanel(C);
            }
        };

        B.ReceiveSignal = (signal) =>
        {
            // ClaimReward();
            RunManager.Instance.Map.FinishNode();
        };

        C.ReceiveSignal = (signal) =>
        {
            // ClaimReward();
            RunManager.Instance.Map.FinishNode();
        };

        runNode.ChangePanel(A);
    }
}
