
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileView : MonoBehaviour, IDropHandler
{
    public TMP_Text InfoText;
    public TMP_Text ManaText;
    public TMP_Text QRText;

    private IndexPath _indexPath;
    public IndexPath IndexPath => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        Tile tile = RunManager.Get<Tile>(_indexPath);
        RunChip runChip = tile.RunChip;

        if (runChip == null)
        {
            InfoText.text = $"";
        }
        else
        {
            ManaText.text = $"";
            InfoText.text = $"{runChip.GetName()}[{runChip.Level}]";
        }

        QRText.text = $"{tile._q}, {tile._r}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null) return;

        if (RunManager.TryUpgradeDanTian(drop.IndexPath, _indexPath)) return;
        if (RunManager.TryApply(drop.IndexPath, _indexPath)) return;
    }
}
