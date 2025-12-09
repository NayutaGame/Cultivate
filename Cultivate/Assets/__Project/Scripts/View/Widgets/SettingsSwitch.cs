
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsSwitch : XView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private CLButtonPatternA PrevButton;
    [SerializeField] private CLButtonPatternA NextButton;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        PrevButton.LeftClickNeuron.Join(Prev);
        NextButton.LeftClickNeuron.Join(Next);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        SwitchModel model = Get<SwitchModel>();

        LabelText.text = model.Name;
        
        ContentText.text = model.GetContentText();
    }

    private void Prev(InteractBehaviour ib, PointerEventData d)
        => Prev();

    private void Prev()
    {
        SwitchModel model = Get<SwitchModel>();
        model.Prev();
        ContentText.text = model.GetContentText();
    }

    private void Next(InteractBehaviour ib, PointerEventData d)
        => Next();

    private void Next()
    {
        SwitchModel model = Get<SwitchModel>();
        model.Next();
        ContentText.text = model.GetContentText();
    }
}
