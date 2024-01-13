
using TMPro;
using UnityEngine;

public class StageEntityView : SimpleView
{
    [SerializeField] private ListView Formations;
    [SerializeField] private ListView Buffs;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ArmorText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        Formations.SetAddress(GetAddress().Append(".Formations"));
        Formations.PointerEnterNeuron.Join(CanvasManager.Instance.FormationAnnotation.PointerEnter, StageManager.Instance.Pause);
        Formations.PointerEnterNeuron.Join(CanvasManager.Instance.FormationAnnotation.PointerExit, StageManager.Instance.Resume);
        Formations.PointerEnterNeuron.Join(CanvasManager.Instance.FormationAnnotation.PointerMove);

        Buffs.SetAddress(GetAddress().Append(".Buffs"));
        Buffs.PointerEnterNeuron.Join(CanvasManager.Instance.BuffAnnotation.PointerEnter, StageManager.Instance.Pause);
        Buffs.PointerEnterNeuron.Join(CanvasManager.Instance.BuffAnnotation.PointerExit, StageManager.Instance.Resume);
        Buffs.PointerEnterNeuron.Join(CanvasManager.Instance.BuffAnnotation.PointerMove);
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
