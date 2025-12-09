
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsButton : XView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private CLButton Button;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        Button.LeftClickNeuron.Join(Click);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        ButtonModel model = Get<ButtonModel>();
        LabelText.text = model.Name;
    }

    private void Click(InteractBehaviour ib, PointerEventData d)
    {
        ButtonModel model = Get<ButtonModel>();
        model.Click();
    }
}
