
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
        Formations.PointerEnterNeuron.Join(StageManager.Instance.Pause);
        Formations.PointerExitNeuron.Join(StageManager.Instance.Resume);

        Buffs.SetAddress(GetAddress().Append(".Buffs"));
        Buffs.PointerEnterNeuron.Join(StageManager.Instance.Pause);
        Buffs.PointerExitNeuron.Join(StageManager.Instance.Resume);
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
