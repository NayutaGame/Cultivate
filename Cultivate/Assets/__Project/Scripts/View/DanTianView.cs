
using UnityEngine;

public class DanTianView : MonoBehaviour
{
    public Transform VLayoutTransform;
    private TileView[] _tileViews;

    public void Configure()
    {
        _tileViews = new TileView[DanTian.DIAMETER * DanTian.DIAMETER];

        for (int y = 0; y < VLayoutTransform.childCount; y++)
        {
            Transform rowTransform = VLayoutTransform.GetChild(y);
            for (int x = 0; x < rowTransform.childCount; x++)
            {
                TileView tileView = rowTransform.GetChild(x).GetComponent<TileView>();

                if(!DanTian.IsInsideXY(x, y)) continue;

                int i = DanTian.DIAMETER * y + x;
                _tileViews[i] = tileView;
                tileView.Configure(new IndexPath($"DanTian.Tiles#{i}"));
            }
        }
    }

    public void Refresh()
    {
        foreach(var view in _tileViews) if(view != null) view.Refresh();
    }
}
