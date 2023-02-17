using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class AcquiredPoolView : MonoBehaviour
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
        int need = RunManager.Get<int>("GetAcquiredChipCount");

        (need, _) = Numeric.Negate(need, current);
        if (need <= 0) return;

        int length = Container.childCount;
        for (int i = length; i < need + length; i++)
        {
            AcquiredChipView v = Instantiate(Prefab, Container).GetComponent<AcquiredChipView>();
            _views.Add(v);
            v.Configure(new IndexPath("TryGetAcquiredChip", i));
        }
    }
}
