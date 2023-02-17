using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryChipView : RunChipView, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public override void Refresh()
    {
        base.Refresh();

        RunChip chip = RunManager.Get<RunChip>(IndexPath);

        gameObject.SetActive(chip != null);
        if (chip == null) return;

        InfoText.text = $"{chip.GetName()}[{chip.Level}]";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ghostGO = Instantiate(gameObject, CanvasManager.Instance.GhostHolder);
        _ghostGO.GetComponent<Image>().raycastTarget = false;
        _ghostTransform = _ghostGO.GetComponent<RectTransform>();

        Vector2 size = _transform.parent.GetComponent<GridLayoutGroup>().cellSize;

        _ghostTransform.sizeDelta = size;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_ghostGO);
        _ghostGO = null;
        _ghostTransform = null;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);

        CanvasManager.Instance.Refresh();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _ghostTransform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null) return;

        if (IndexPath.Equals(drop.IndexPath))
        {
            Debug.Log("same object");
            return;
        }

        if (drop.IndexPath._str == "TryGetRunChip")
        {
            if (RunManager.TryUpgradeInventory(drop.IndexPath, IndexPath)) return;
            if (RunManager.Swap(drop.IndexPath, IndexPath)) return;
        }
    }
}
