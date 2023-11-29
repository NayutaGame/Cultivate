
using TMPro;
using UnityEngine;

public class StageEntityView : MonoBehaviour, IAddress
{
    public InteractDelegate InteractDelegate;
    [SerializeField] private ListView Formations;
    [SerializeField] private ListView Buffs;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ArmorText;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void SetAddress(Address address)
    {
        _address = address;

        Formations.SetAddress(_address.Append(".Formations"));
        Buffs.SetAddress(_address.Append(".Buffs"));
    }

    public virtual void Refresh()
    {
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
