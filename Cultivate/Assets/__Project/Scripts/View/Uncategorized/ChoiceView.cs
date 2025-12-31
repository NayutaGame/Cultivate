
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceView : XView
{
    [SerializeField] public TMP_Text Text;
    [SerializeField] public CLButton Button;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        Button.LeftClickNeuron.Join(MakeChoice);
    }

    public override void Refresh()
    {
        base.Refresh();

        ChoiceOption choiceOption = Get<ChoiceOption>();
        Text.text = choiceOption.Text;
    }

    private void MakeChoice(InteractBehaviour ib, PointerEventData d)
    {
        Get<ChoiceOption>().MakeChoice();
    }
}