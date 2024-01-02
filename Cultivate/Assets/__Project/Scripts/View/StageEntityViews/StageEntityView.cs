
using TMPro;
using UnityEngine;

public class StageEntityView : AddressBehaviour
{
    [SerializeField] private ListView Formations;
    [SerializeField] private ListView Buffs;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ArmorText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        Formations.SetAddress(GetAddress().Append(".Formations"));
        Formations.PointerEnterNeuron.Set(CanvasManager.Instance.FormationAnnotation.SetAddressFromIB, StageManager.Instance.Pause);
        Formations.PointerEnterNeuron.Set(CanvasManager.Instance.FormationAnnotation.SetAddressToNull, StageManager.Instance.Resume);
        Formations.PointerEnterNeuron.Set(CanvasManager.Instance.FormationAnnotation.UpdateMousePos);

        Buffs.SetAddress(GetAddress().Append(".Buffs"));
        Buffs.PointerEnterNeuron.Set(CanvasManager.Instance.BuffAnnotation.SetAddressFromIB, StageManager.Instance.Pause);
        Buffs.PointerEnterNeuron.Set(CanvasManager.Instance.BuffAnnotation.SetAddressToNull, StageManager.Instance.Resume);
        Buffs.PointerEnterNeuron.Set(CanvasManager.Instance.BuffAnnotation.UpdateMousePos);
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
}
