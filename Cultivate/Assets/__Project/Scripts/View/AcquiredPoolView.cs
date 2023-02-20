using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class AcquiredPoolView : MonoBehaviour, IDropHandler
{
    public Transform Container;
    public GameObject Prefab;

    private List<AcquiredChipView> _views;

    public void Configure()
    {
        _views = new List<AcquiredChipView>();
    }

    public void Refresh()
    {
        PopulateList();
        foreach(var view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        int current = Container.childCount;
        int need = RunManager.Instance.GetUnequippedCount();

        (need, _) = Numeric.Negate(need, current);
        if (need <= 0) return;

        int length = Container.childCount;
        for (int i = length; i < need + length; i++)
        {
            AcquiredChipView v = Instantiate(Prefab, Container).GetComponent<AcquiredChipView>();
            _views.Add(v);
            v.Configure(new IndexPath("TryGetUnequipped", i));
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (drop.IndexPath._str == "TryGetUnequipped")
            return;

        if (drop.IndexPath._str == "GetHeroNeiGong")
        {
            RunManager.Instance.UnequipNeiGong(drop.IndexPath);
            return;
        }

        if (drop.IndexPath._str == "GetHeroWaiGong")
        {
            RunManager.Instance.UnequipWaiGong(drop.IndexPath);
            return;
        }
    }
}
