using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class TechTreeView : MonoBehaviour
{
    private static readonly int HEIGHT = 8;

    public Transform Container;
    public GameObject Prefab;

    private IInventory _inventory;
    private List<Transform> _holders;
    private List<TechView> _views;

    public virtual void Configure(IInventory inventory)
    {
        _inventory = inventory;
        InitHolders();
        _views = new ();
        PopulateList();
    }

    public virtual void Refresh()
    {
        foreach(TechView view in _views) view.Refresh();
    }

    private void InitHolders()
    {
        _holders = new();
        for (int x = 0; x < Container.childCount; x++)
        {
            Transform childTransform = Container.GetChild(x);
            if (childTransform.name != "VLayout") continue;

            for (int y = 0; y < childTransform.childCount; y++)
            {
                Transform grandchildTransform = childTransform.GetChild(y);

                _holders.Add(grandchildTransform);
            }
        }
    }

    private void PopulateList()
    {
        for (int i = 0; i < _inventory.GetCount(); i++)
        {
            IndexPath indexPath = new IndexPath(_inventory.GetIndexPathString(), i);
            RunTech tech = RunManager.Get<RunTech>(indexPath);
            Vector2Int position = tech.GetPosition();
            int index = position.x * HEIGHT + position.y;
            Transform parent = _holders[index];

            TechView v = Instantiate(Prefab, parent).GetComponent<TechView>();
            _views.Add(v);
            v.Configure(indexPath);
        }
    }
}
