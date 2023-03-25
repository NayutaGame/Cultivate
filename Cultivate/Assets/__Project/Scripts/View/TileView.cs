
using System.Text;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public class TileView : MonoBehaviour, IIndexPath, IDropHandler
{
    private Image _image;

    // public TMP_Text YieldText;
    public TMP_Text QRText;
    public TMP_Text PrimaryText;
    // public TMP_Text TerrainText;
    // public TMP_Text SlotIndexText;
    // public TMP_Text WorkerLockText;
    public TMP_Text PowerText;
    public TMP_Text LevelText;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _image = GetComponent<Image>();
        _indexPath = indexPath;
    }

    public void Refresh()
    {
        Tile tile = RunManager.Get<Tile>(_indexPath);
        gameObject.SetActive(tile.Revealed);
        if (!tile.Revealed)
            return;

        QRText.text = $"{tile._q}, {tile._r}";
        if (tile.AcquiredRunChip == null)
        {
            PrimaryText.text = "";
            LevelText.text = "";
            SetColorFromJingJie(JingJie.LianQi);
        }
        else
        {
            PrimaryText.text = tile.AcquiredRunChip.GetName();
            LevelText.text = tile.AcquiredRunChip.Chip.Level.ToString();
            SetColorFromJingJie(tile.AcquiredRunChip.GetJingJie());
        }

        if (tile.AcquiredRunChip == null || !(tile.AcquiredRunChip.Chip._entry is WuXingChipEntry))
        {
            PowerText.text = tile.GetPowerString();
        }
        else
        {
            PowerText.text = "";
        }

        // StringBuilder sb = new StringBuilder();
        // if (tile.XiuWei > 0)
        //     sb.Append($"{tile.XiuWei}修 ");
        // if (tile.ChanNeng > 0)
        //     sb.Append($"{tile.ChanNeng}产");
        // YieldText.text = sb.ToString();
        //
        // if (tile.Building != null)
        // {
        //     PrimaryText.text = tile.Building.GetName();
        // }
        // else if (tile.Resource != null)
        // {
        //     PrimaryText.text = tile.Resource.GetName();
        // }
        // else
        // {
        //     PrimaryText.text = "";
        // }
        //
        // TerrainText.text = "";
        // SlotIndexText.text = tile.SlotIndex == null ? "" : tile.SlotIndex.Value.ToString();
        // WorkerLockText.text = tile.WorkerLock == null ? "" : "锁";
        //
        // // color part
        // CharacterPanelState state = RunCanvas.Instance.CharacterPanel._state;
        // if (state is CharacterPanelStateNormal normal)
        // {
        //     bool hasWorker = tile.Worker != null;
        //     _image.color = !hasWorker ? Color.white : Color.cyan;
        // }
        // else if (state is CharacterPanelStateDragProductCell drag)
        // {
        //     bool canDrop = RunManager.Instance.CanDropProduct(drag.Item.GetIndexPath(), _indexPath);
        //     _image.color = canDrop ? Color.green : Color.red;
        // }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;

        if (RunManager.Instance.TryUpgradeDanTian(drop.GetIndexPath(), _indexPath)) return;
        if (RunManager.Instance.TryPlug(drop.GetIndexPath(), _indexPath)) return;
        // if (RunManager.Instance.TryDropProduct(drop.GetIndexPath(), _indexPath)) return;
    }

    protected void SetColorFromJingJie(JingJie jingJie)
    {
        _image.color = CanvasManager.Instance.JingJieColors[jingJie];
    }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     RunManager.Instance.TryToggleWorkerLock(eventData.pointerClick.GetComponent<IIndexPath>().GetIndexPath());
    //     RunCanvas.Instance.Refresh();
    // }
}
