
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceView : XView
{
    [SerializeField] public TMP_Text Text;

    public override void Refresh()
    {
        base.Refresh();

        ChoiceOption choiceOption = Get<ChoiceOption>();
        Text.text = choiceOption.Text;
    }
}