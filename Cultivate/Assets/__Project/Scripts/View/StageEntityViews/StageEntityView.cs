
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageEntityView : LegacyAddressBehaviour
{
    [SerializeField] private ListView Formations;
    [SerializeField] private ListView Buffs;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ArmorText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        Formations.SetAddress(GetAddress().Append(".Formations"));
        Formations.PointerEnterNeuron.Set(PointerEnterBuffNeuron);
        Formations.PointerEnterNeuron.Set(PointerExitBuffNeuron);
        Formations.PointerEnterNeuron.Set(PointerMoveBuffNeuron);

        Buffs.SetAddress(GetAddress().Append(".Buffs"));
        Buffs.PointerEnterNeuron.Set(PointerEnterFormationNeuron);
        Buffs.PointerEnterNeuron.Set(PointerExitFormationNeuron);
        Buffs.PointerEnterNeuron.Set(PointerMoveFormationNeuron);
    }

    public override void Refresh()
    {
        base.Refresh();

        Formations.Refresh();
        Buffs.Refresh();

        StageEntity entity = Get<StageEntity>();
        HealthText.text = $"{entity.Hp}/{entity.MaxHp}";
        if (entity.Armor == 0)
        {
            ArmorText.text = "";
        }
        else
        {
            ArmorText.text = $"{entity.Armor}";
        }
    }

    public Neuron<InteractBehaviour, PointerEventData> PointerEnterBuffNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitBuffNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveBuffNeuron = new();

    public Neuron<InteractBehaviour, PointerEventData> PointerEnterFormationNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitFormationNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveFormationNeuron = new();
}
