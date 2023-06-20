using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public class MapPanel : Panel, IIndexPath
{
    private static readonly int HEIGHT = 3;
    private static readonly int WIDTH = 10;

    public RectTransform _backgroundTransform;

    public Transform Container;

    private NodeView[] _views;
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public override Tween GetShowTween()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(_backgroundTransform.DOAnchorPosX(4f, 0.3f).SetEase(Ease.OutQuad));

    public override Tween GetHideTween()
        => DOTween.Sequence()
            .Append(_backgroundTransform.DOAnchorPosX(1924f, 0.3f).SetEase(Ease.InQuad))
            .AppendCallback(() => gameObject.SetActive(false));

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;

        _views = new NodeView[HEIGHT * WIDTH];
        PopulateList();
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
                _views[i].Configure(new IndexPath($"{_indexPath}.Nodes#{i}"));
            }
        }
    }
}
