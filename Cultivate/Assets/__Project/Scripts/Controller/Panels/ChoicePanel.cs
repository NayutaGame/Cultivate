
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoicePanel : Panel
{
    [SerializeField] private ListView ChoiceList;
    
    private Address _address;

    public override void AwakeFunction()
    {
        _address = new Address("Run.Environment.ActivePanel");
        ChoiceList.SetAddress(_address.Append(".ChoiceList"));
        base.AwakeFunction();
        
        ChoiceList.LeftClickNeuron.Add(MakeChoice);

        // Buttons[0].onClick.RemoveAllListeners();
        // Buttons[1].onClick.RemoveAllListeners();
        // Buttons[2].onClick.RemoveAllListeners();
        // Buttons[3].onClick.RemoveAllListeners();
        //
        // Buttons[0].onClick.AddListener(SelectOption0);
        // Buttons[1].onClick.AddListener(SelectOption1);
        // Buttons[2].onClick.AddListener(SelectOption2);
        // Buttons[3].onClick.AddListener(SelectOption3);
        //
        // Buttons[0].onClick.AddListener(AudioManager.PlayButtonPress);
        // Buttons[1].onClick.AddListener(AudioManager.PlayButtonPress);
        // Buttons[2].onClick.AddListener(AudioManager.PlayButtonPress);
        // Buttons[3].onClick.AddListener(AudioManager.PlayButtonPress);
        //
        // PropagatePointerEnters[0]._onPointerEnter = AudioManager.PlayButtonHover;
        // PropagatePointerEnters[1]._onPointerEnter = AudioManager.PlayButtonHover;
        // PropagatePointerEnters[2]._onPointerEnter = AudioManager.PlayButtonHover;
        // PropagatePointerEnters[3]._onPointerEnter = AudioManager.PlayButtonHover;
    }

    private void MakeChoice(InteractBehaviour ib, PointerEventData d)
    {
        ib.Get<ChoiceOption>().MakeChoice();
    }
}