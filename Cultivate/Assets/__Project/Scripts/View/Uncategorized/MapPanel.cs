
using DG.Tweening;
using UnityEngine;

public class MapPanel : Panel, IAddress
{
    [SerializeField] private CanvasGroup CanvasGroup;
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
            .AppendCallback(() => gameObject.SetActive(true));

    public override Tween HideAnimation()
        => DOTween.Sequence()
            .AppendCallback(PlaySFX)
            .AppendCallback(() => gameObject.SetActive(false));

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
