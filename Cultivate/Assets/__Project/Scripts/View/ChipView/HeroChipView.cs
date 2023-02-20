using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroChipView : RunChipView, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public override void Refresh()
    {
        base.Refresh();

        RunChip chip = RunManager.Get<RunChip>(IndexPath);

        gameObject.SetActive(true);
        if(chip == null)
        {
            InfoText.text = "空";
            return;
        }
        else
        {
            InfoText.text = $"{chip.GetName()}[{chip.Level}]";
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ghostGO = Instantiate(gameObject, CanvasManager.Instance.GhostHolder);
        _ghostGO.GetComponent<Image>().raycastTarget = false;
        _ghostTransform = _ghostGO.GetComponent<RectTransform>();

        Vector2 size = _ghostTransform.sizeDelta;

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

        if (drop.IndexPath._str == "TryGetAcquiredChip")
        {
            if (IndexPath._str == "GetHeroNeiGong")
            {
                RunManager.TryEquipNeiGong(drop.IndexPath, IndexPath);
            }
            else if (IndexPath._str == "GetHeroWaiGong")
            {
                RunManager.TryEquipWaiGong(drop.IndexPath, IndexPath);
            }
            return;
        }

        if (drop.IndexPath._str == IndexPath._str)
        {
            if (IndexPath._str == "GetHeroNeiGong")
            {
                RunManager.SwapNeiGong(drop.IndexPath, IndexPath);
            }
            else if (IndexPath._str == "GetHeroWaiGong")
            {
                RunManager.SwapWaiGong(drop.IndexPath, IndexPath);
            }
            return;
        }
    }
}
