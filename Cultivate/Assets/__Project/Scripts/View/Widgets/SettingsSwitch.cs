
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitch : LegacySimpleView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button NextButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        PrevButton.onClick.RemoveAllListeners();
        PrevButton.onClick.AddListener(Prev);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(Next);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        SwitchModel model = Get<SwitchModel>();

        LabelText.text = model.Name;
        
        ContentText.text = model.GetContentText();
    }

    private void Prev()
    {
        SwitchModel model = Get<SwitchModel>();
        model.Prev();
        ContentText.text = model.GetContentText();
    }

    private void Next()
    {
        SwitchModel model = Get<SwitchModel>();
        model.Next();
        ContentText.text = model.GetContentText();
    }
}
