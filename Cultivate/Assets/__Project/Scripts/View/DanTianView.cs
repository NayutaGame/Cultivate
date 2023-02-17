
using UnityEngine;

public class DanTianView : MonoBehaviour
{
    public Transform VLayoutTransform;
    private TileView[] _tileViews;

    public void Configure()
    {
        _tileViews = new TileView[DanTian.WIDTH * DanTian.WIDTH];

        for (int y = 0; y < VLayoutTransform.childCount; y++)
        {
            Transform rowTransform = VLayoutTransform.GetChild(y);
            for (int x = 0; x < rowTransform.childCount; x++)
            {
                TileView tileView = rowTransform.GetChild(x).GetComponent<TileView>();

                if(!DanTian.IsInside(x, y)) continue;

                _tileViews[DanTian.WIDTH * y + x] = tileView;
                tileView.Configure(new IndexPath("GetTileXY", x, y));
            }
        }
    }

    public void Refresh()
    {
        foreach(var view in _tileViews) if(view != null) view.Refresh();
    }
}
