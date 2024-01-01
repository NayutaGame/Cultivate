
using TMPro;
using UnityEngine;

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
        Buffs.SetAddress(GetAddress().Append(".Buffs"));
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

    // TODO: to be removed
    #region IInteractable

    private InteractHandler _interactHandler;
    public InteractHandler GetHandler() => _interactHandler;
    public void SetHandler(InteractHandler interactHandler)
    {
        _interactHandler = interactHandler;
        Formations.SetHandler(_interactHandler);
        Buffs.SetHandler(_interactHandler);
    }

    #endregion
}
