
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : LegacySimpleView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private Button Button;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Click);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        ButtonModel model = Get<ButtonModel>();
        LabelText.text = model.Name;
    }

    public void Click()
    {
        ButtonModel model = Get<ButtonModel>();
        model.Click();
    }
}
