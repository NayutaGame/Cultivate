
using DG.Tweening;
using UnityEngine;

public class MapPanel : Panel, IAddress
{
    [SerializeField] private ListView StepItemListView;
    [SerializeField] private CanvasGroup CanvasGroup;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();
    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public override Tween ShowAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(Refresh)
            .AppendCallback(PlaySFX)
            .Append(CanvasManager.Instance.Curtain.HideAnimation())
            .Append(_rectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CanvasGroup.DOFade(1f, 0.15f));
    }

    public override Tween HideAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(PlaySFX)
            .Append(_rectTransform.DOScale(1.2f, 0.15f).SetEase(Ease.OutQuad))
            .Join(CanvasGroup.DOFade(0f, 0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }

    public override void Configure()
    {
        base.Configure();
        _address = new Address("Run.Environment.Map");
        StepItemListView.SetAddress(_address.Append(".StepItems"));
    }

    public override void Refresh()
    {
        base.Refresh();
        StepItemListView.Refresh();
    }

    private void PlaySFX()
    {
    }
}
