using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class InspectorView : MonoBehaviour
{
    // public Transform Container;
    // public GameObject Prefab;
    //
    // private List<ProductCellView> _views;
    //
    // public void Configure()
    // {
    //     _views = new List<ProductCellView>();
    // }
    //
    // public void Refresh()
    // {
    //     PopulateList();
    //     foreach(var view in _views) view.Refresh();
    // }
    //
    // private void PopulateList()
    // {
    //     int current = Container.childCount;
    //     int need = RunManager.Instance.GetProductCount();
    //
    //     (need, _) = Numeric.Negate(need, current);
    //     if (need <= 0) return;
    //
    //     int length = Container.childCount;
    //     for (int i = length; i < need + length; i++)
    //     {
    //         ProductCellView v = Instantiate(Prefab, Container).GetComponent<ProductCellView>();
    //         _views.Add(v);
    //         v.Configure(new IndexPath("TryGetProduct", i));
    //     }
    // }
}
