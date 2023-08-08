
using TMPro;
using UnityEngine;

public class MechView : MonoBehaviour, IIndexPath, IInteractable
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate()
        => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
        => InteractDelegate = interactDelegate;

    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public virtual void Refresh()
    {
        Mech mech = DataManager.Get<Mech>(GetIndexPath());

        SetMechType(mech.GetMechType());
        SetCount(mech.Count);
    }

    private void SetMechType(MechType mechType)
    {
        NameText.text = mechType.ToString();
    }

    private void SetCount(int count)
    {
        CountText.text = count.ToString();
    }
}
