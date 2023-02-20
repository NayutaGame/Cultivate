using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public Transform Container;
    public GameObject Prefab;

    public Button RefreshChipButton;
    public Button UpgradeFirstChipButton;

    private List<RunChipView> _views;

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
