
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : Panel
{
    public TMP_Text DetailedText;
    public Button[] Buttons;
    public TMP_Text[] Texts;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        Buttons[0].onClick.RemoveAllListeners();
        Buttons[1].onClick.RemoveAllListeners();
        Buttons[2].onClick.RemoveAllListeners();
        Buttons[3].onClick.RemoveAllListeners();

        Buttons[0].onClick.AddListener(SelectOption0);
        Buttons[1].onClick.AddListener(SelectOption1);
        Buttons[2].onClick.AddListener(SelectOption2);
        Buttons[3].onClick.AddListener(SelectOption3);
    }

    public override void Refresh()
    {
        base.Refresh();

        DialogPanelDescriptor d = _address.Get<DialogPanelDescriptor>();

        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < Buttons.Length; i++)
        {
            bool active = i < d.GetOptionsCount() && !RunManager.Instance.Environment.Map.Selecting;
            Buttons[i].gameObject.SetActive(active);
            if(!active)
                continue;

            Buttons[i].interactable = d.GetOption(i).CanSelect();
            Texts[i].text = d.GetOption(i).Text;
        }
    }

    private void SelectedOption(int i)
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new SelectedOptionSignal(i));
        if (RunManager.Instance.Environment == null)
            return;
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }

    private void SelectOption0() => SelectedOption(0);
    private void SelectOption1() => SelectedOption(1);
    private void SelectOption2() => SelectedOption(2);
    private void SelectOption3() => SelectedOption(3);
}
