
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public abstract class ListView<T> : MonoBehaviour, IIndexPath, IInteractable where T : IIndexPath
{
    public Transform Container;
    public GameObject Prefab;

    public virtual GameObject GetPrefab(object element)
        => Prefab;

    private List<T> _views;
    public List<T> Views => _views;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;

    public S GetModel<S>()
        => DataManager.Get<S>(GetIndexPath());

    public virtual void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;

        _views.Do(
            v => {
                if (v is IInteractable interactable)
                    interactable.SetDelegate(InteractDelegate);
            });
    }

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
        _views = new List<T>();
        RegisterExists();
    }

    public virtual void Refresh()
    {
        PopulateList();
        foreach(T view in _views) view.Refresh();
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            T view = Container.GetChild(i).GetComponent<T>();
            RegisterNew(view, i);
        }
    }

    private void RegisterNew(T view, int i)
    {
        if (!_views.Contains(view))
            _views.Add(view);

        view.Configure(new IndexPath($"{_indexPath}#{i}"));

        if (view is IInteractable interactable)
            interactable.SetDelegate(InteractDelegate);
    }

    private void PopulateList()
    {
        int current = Container.childCount;
        IList inventory = GetModel<IList>();
        int need = inventory.Count;

        (need, _) = Numeric.Negate(need, current);
        if (need <= 0) return;

        int length = Container.childCount;
        for (int i = length; i < need + length; i++)
        {
            GameObject prefab = GetPrefab(inventory[i]);
            T view = Instantiate(prefab, Container).GetComponent<T>();
            RegisterNew(view, i);
        }
    }
}
