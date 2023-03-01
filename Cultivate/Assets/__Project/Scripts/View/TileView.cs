
using System.Text;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class TileView : MonoBehaviour, IDropHandler
{
    public TMP_Text InfoText;
    public TMP_Text ElementsText;
    public TMP_Text QRText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh()
    {
        Tile tile = RunManager.Get<Tile>(_indexPath);

        gameObject.SetActive(tile.Revealed);
        if (!tile.Revealed)
        {
            return;
        }

        RunChip runChip = tile.Chip;

        if (runChip == null)
        {
            InfoText.text = $"";
        }
        else
        {
            InfoText.text = $"{runChip.GetName()}[{runChip.Level}]";
        }

        StringBuilder sb = new StringBuilder();
        WuXing.Traversal.Do(wuXing =>
        {
            int count = tile._elements[wuXing];
            if(count != 0)
                sb.Append($"{count}{wuXing.ToString()}\t");
        });
        ElementsText.text = sb.ToString();
        QRText.text = $"{tile._q}, {tile._r}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null) return;

        if (RunManager.Instance.TryUpgradeDanTian(drop.IndexPath, _indexPath)) return;
        if (RunManager.Instance.TryApply(drop.IndexPath, _indexPath)) return;
        if (RunManager.Instance.TryXiuLian(drop.IndexPath, _indexPath)) return;
    }
}
