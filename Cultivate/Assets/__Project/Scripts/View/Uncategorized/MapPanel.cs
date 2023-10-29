
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : Panel, IAddress
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    public RectTransform _backgroundTransform;

    public Image Mask;

    public Transform Container;

    private NodeView[] _views;

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
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(_backgroundTransform.DOAnchorPosX(4f, 0.3f).SetEase(Ease.OutQuad))
            .Join(Mask.DOFade(0.8f, 0.4f).SetEase(Ease.OutQuad));

    public override Tween HideAnimation()
        => DOTween.Sequence()
            .AppendCallback(PlaySFX)
            .Append(Mask.DOFade(0f, 0.4f).SetEase(Ease.InQuad))
            .Join(_backgroundTransform.DOAnchorPosX(1924f, 0.3f).SetEase(Ease.InQuad))
            .AppendCallback(() => gameObject.SetActive(false));

    public override void Configure()
    {
        _address = new Address("Run.Environment.Map");

        _views = new NodeView[HEIGHT * WIDTH];
        PopulateList();

        Mask.GetComponent<Button>().onClick.RemoveAllListeners();
        Mask.GetComponent<Button>().onClick.AddListener(() => SetShowing(false));
    }

    public override void Refresh()
    {
        foreach(NodeView view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        for (int x = 0; x < Container.childCount; x++)
        {
            Transform levelTransform = Container.GetChild(x);

            for (int y = 0; y < levelTransform.childCount; y++)
            {
                int i = x * HEIGHT + y;
                _views[i] = levelTransform.GetChild(y).GetComponent<NodeView>();
                _views[i].SetAddress(_address.Append($".Nodes#{i}"));
            }
        }
    }

    private void PlaySFX()
    {
    }
}
