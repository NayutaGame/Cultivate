
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
        
        ChoiceList.NeuronBundle.LeftClickNeuron.Add(MakeChoice);
    }

    private void MakeChoice(InteractBehaviour ib, PointerEventData d)
    {
        ib.Get<ChoiceOption>().MakeChoice();
    }
}