using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChipInventoryView : MonoBehaviour, IDropHandler
{
    public Transform Container;
    public GameObject Prefab;

    private List<RunChipView> _views;

    public Button RefreshChipButton;
    public Button UpgradeFirstChipButton;

    public void Configure()
    {
        _views = new List<RunChipView>();
        RefreshChipButton.onClick.AddListener(RefreshChip);
        UpgradeFirstChipButton.onClick.AddListener(UpgradeFirstChip);
    }

    public void Refresh()
    {
        PopulateList();
        foreach(var view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        int current = Container.childCount;
        int need = RunManager.Instance.GetRunChipCount();

        (need, _) = Numeric.Negate(need, current);
        if (need <= 0) return;

        int length = Container.childCount;
        for (int i = length; i < need + length; i++)
        {
            RunChipView v = Instantiate(Prefab, Container).GetComponent<RunChipView>();
            _views.Add(v);
            v.Configure(new IndexPath("TryGetRunChip", i));
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (drop.GetIndexPath()._str == "GetHeroWaiGong")
        {
            RunManager.Instance.Unequip(drop.GetIndexPath());
            return;
        }
    }

    public void RefreshChip()
    {
        RunManager.Instance.RefreshChip();
        Refresh();
    }

    public void UpgradeFirstChip()
    {
        RunManager.Instance.UpgradeFirstChip();
        Refresh();
    }
}
