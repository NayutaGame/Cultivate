using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : Panel
{
    public TMP_Text DetailedText;
    public Button[] Buttons;
    public TMP_Text[] Texts;

    public override void Configure()
    {
        base.Configure();

        Buttons[0].onClick.AddListener(SelectOption0);
        Buttons[1].onClick.AddListener(SelectOption1);
        Buttons[2].onClick.AddListener(SelectOption2);
        Buttons[3].onClick.AddListener(SelectOption3);
    }

    public override void Refresh()
    {
        base.Refresh();

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        DialogPanelDescriptor d = runNode.CurrentPanel as DialogPanelDescriptor;

        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < Buttons.Length; i++)
        {
            bool active = i < d.GetOptionsCount() && !RunManager.Instance.Map.Selecting;
            Buttons[i].gameObject.SetActive(active);
            if(!active)
                continue;

            Texts[i].text = d.GetOption(i);
        }
    }

    private void SelectedOption(int i)
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        DialogPanelDescriptor d = runNode.CurrentPanel as DialogPanelDescriptor;

        d.ReceiveSignal(new SelectedOptionSignal(i));
        RunCanvas.Instance.NodePanel.Refresh();
    }

    private void SelectOption0() => SelectedOption(0);
    private void SelectOption1() => SelectedOption(1);
    private void SelectOption2() => SelectedOption(2);
    private void SelectOption3() => SelectedOption(3);
}
