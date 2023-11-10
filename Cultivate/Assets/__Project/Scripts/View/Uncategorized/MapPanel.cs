
using DG.Tweening;
using UnityEngine;

public class MapPanel : Panel, IAddress
{
    [SerializeField] private ListView StepItemListView;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();
    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public override Tween ShowAnimation()
        => DOTween.Sequence()
            .AppendCallback(Refresh)
            .AppendCallback(PlaySFX)
            .Append(base.ShowAnimation());

    public override Tween HideAnimation()
        => DOTween.Sequence()
            .AppendCallback(PlaySFX)
            .Append(base.HideAnimation());

    public override void Configure()
    {
        _address = new Address("Run.Environment.Map");
        StepItemListView.SetAddress(_address.Append(".StepItems"));
    }

    public override void Refresh()
    {
        StepItemListView.Refresh();
    }

    private void PlaySFX()
    {
    }
}
