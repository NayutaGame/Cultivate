
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewListView : XView
{
    private RectTransform SlotContainer;
    private RectTransform ViewContainer;
    [SerializeField] private GameObject[] Prefabs;

    private List<ItemBehaviour> _activePool;
    private List<ItemBehaviour>[] _inactivePools;

    #region Accessors
    
    public List<ItemBehaviour> ActivePool => _activePool;
    public List<ItemBehaviour>[] InactivePools => _inactivePools;

    public IEnumerable<T> Traversal<T>() where T : XBehaviour
    {
        foreach (var itemBehaviour in _activePool)
            yield return itemBehaviour.GetBehaviour<T>();
        foreach (var itemBehaviourList in _inactivePools)
        foreach (var itemBehaviour in itemBehaviourList)
            yield return itemBehaviour.GetBehaviour<T>();
    }

    public IEnumerable<T> TraversalActive<T>() where T : XBehaviour
    {
        foreach (var itemBehaviour in _activePool)
            yield return itemBehaviour.GetBehaviour<T>();
    }

    public int? IndexFromItemBehaviour(ItemBehaviour toGetIndex)
    {
        if (toGetIndex == null)
            return null;
        return _activePool.FirstIdx(itemBehaviour => itemBehaviour == toGetIndex);
    }

    public ItemBehaviour ItemBehaviourFromIndex(int i)
        => _activePool[i];

    #endregion

    #region List Operations
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();

        SlotContainer = transform.GetChild(0).GetComponent<RectTransform>();
        ViewContainer = transform.GetChild(1).GetComponent<RectTransform>();
        
        _activePool = new List<ItemBehaviour>();
        _inactivePools = new List<ItemBehaviour>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<ItemBehaviour>();

        RegisterExists();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        CheckAwake();
        Sync();
    }

    public override void Refresh()
    {
        base.Refresh();
        _activePool.Do(itemBehaviour =>
            itemBehaviour.Refresh());
    }

    public void Sync()
    {
        DisableAllItems();

        _model = Get<IListModel>();
        for (int i = 0; i < _model.Count(); i++)
            InsertItem(i, GetAddress().Append($"#{i}").Get<object>());

        Refresh();
    }

    private void RegisterExists()
    {
        int existsCount = SlotContainer.childCount;
        for (int i = 0; i < existsCount; i++)
        {
            GameObject viewGo = SlotContainer.GetChild(0).gameObject;
            ItemBehaviour itemBehaviour = viewGo.GetOrAddComponent<ItemBehaviour>();
            CreateSlot(viewGo, i);
            
            itemBehaviour.AwakeFunction(viewGo.GetComponent<XView>());
            InitItem(itemBehaviour, itemBehaviour.PrefabIndex);
        }
    }

    private ItemBehaviour AllocItem(int prefabIndex)
        => Instantiate(Prefabs[prefabIndex], ViewContainer).GetComponent<ItemBehaviour>();

    private void CreateSlot(GameObject viewGo, int index)
    {
        GameObject slotGo = new GameObject($"Slot#{index}", typeof(RectTransform), typeof(Image), typeof(InteractBehaviour));
        RectTransform slotRt = slotGo.GetComponent<RectTransform>();
        Image slotImage = slotGo.GetComponent<Image>();
        InteractBehaviour slotIb = slotGo.GetComponent<InteractBehaviour>();

        XView view = viewGo.GetComponent<XView>();
        RectTransform viewRt = viewGo.GetComponent<RectTransform>();
        
        slotGo.transform.SetParent(SlotContainer);
        slotRt.anchorMin = new Vector2(0f, 1f);
        slotRt.anchorMax = new Vector2(0f, 1f);
        slotRt.pivot = new Vector2(0.5f, 0.5f);

        slotRt.localScale = viewRt.localScale;
        slotRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewRt.rect.width);
        slotRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, viewRt.rect.height);

        Debug.Log(viewRt.position);
        slotRt.position = viewRt.position;
        
        viewGo.transform.SetParent(ViewContainer);
        viewRt.position = slotRt.position;
        viewRt.anchorMin = new Vector2(0.5f, 0.5f);
        viewRt.anchorMax = new Vector2(0.5f, 0.5f);
        viewRt.pivot = new Vector2(0.5f, 0.5f);

        // slotImage.color = new Color(0, 0, 0, 0);
        slotImage.color = new Color(1, 1, 1, 1);
        
        slotIb.SetView(view);
        view.SetInteractBehaviour(slotIb);
    }

    protected virtual void InitItem(ItemBehaviour itemBehaviour, int prefabIndex)
    {
        InteractBehaviour ib = itemBehaviour.GetInteractBehaviour();
        itemBehaviour.GetComponent<XView>().AwakeFunction();
        itemBehaviour.gameObject.SetActive(false);
        
        if (ib != null)
            ib.gameObject.SetActive(false);

        itemBehaviour.PrefabIndex = prefabIndex;
        _inactivePools[prefabIndex].Add(itemBehaviour);
        BindInteractBehaviour(ib);
        itemBehaviour.name = $"View#{Traversal<ItemBehaviour>().Count() - 1}";
    }

    private ItemBehaviour EnableItem(int prefabIndex, int orderInPool, int index)
    {
        List<ItemBehaviour> pool = _inactivePools[prefabIndex];
        ItemBehaviour itemBehaviour = pool[orderInPool];

        pool.RemoveAt(orderInPool);
        _activePool.Insert(index, itemBehaviour);

        for (int i = index; i < _activePool.Count; i++)
        {
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));
            _activePool[i].Refresh();
        }

        itemBehaviour.transform.SetSiblingIndex(index);
        itemBehaviour.gameObject.SetActive(true);
        
        
        InteractBehaviour ib = itemBehaviour.GetInteractBehaviour();
        if (ib != null)
            ib.gameObject.SetActive(true);

        return itemBehaviour;
    }

    private ItemBehaviour DisableItem(int index)
    {
        ItemBehaviour itemBehaviour = _activePool[index];

        itemBehaviour.gameObject.SetActive(false);
        itemBehaviour.transform.SetAsLastSibling();

        _activePool.RemoveAt(index);
        _inactivePools[itemBehaviour.PrefabIndex].Add(itemBehaviour);

        for (int i = index; i < _activePool.Count; i++)
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));

        return itemBehaviour;
    }

    private void DisableAllItems()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            ItemBehaviour itemBehaviour = _activePool[index];

            itemBehaviour.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemBehaviour.PrefabIndex].Insert(0, itemBehaviour);
        }
    }
    
    private int FetchItemBehaviour(int prefabIndex)
    {
        List<ItemBehaviour> pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
            return 0;

        ItemBehaviour itemBehaviour = AllocItem(prefabIndex);
        InitItem(itemBehaviour, prefabIndex);
        return 0;
    }

    #endregion

    #region Prefab Provider

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider) => _prefabProvider = prefabProvider;
    private GameObject GetPrefab(object item) => Prefabs[_prefabProvider?.Invoke(item) ?? 0];
    protected int GetPrefabIndex(object model) => _prefabProvider?.Invoke(model) ?? 0;

    #endregion

    #region Staging

    private IListModel _model;

    public async UniTask InsertItemStaging(int index, object item)
    {
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItem(prefabIndex, orderInPool, index);
    }

    public void InsertItem(int index, object item)
    {
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItem(prefabIndex, orderInPool, index);
    }

    public async UniTask RemoveAtStaging(int index)
    {
        DisableItem(index);
    }

    public async UniTask ModifiedStaging(int index)
    {
        _activePool[index].Refresh();
    }

    #endregion

    #region Interact Behaviour

    public Neuron<InteractBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> DroppingNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();

    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingEnterNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingExitNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingMoveNeuron = new();

    private void BindInteractBehaviour(InteractBehaviour ib)
    {
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(PointerEnterNeuron);
        ib.PointerExitNeuron.Join(PointerExitNeuron);
        ib.PointerMoveNeuron.Join(PointerMoveNeuron);
        ib.BeginDragNeuron.Join(BeginDragNeuron);
        ib.EndDragNeuron.Join(EndDragNeuron);
        ib.DragNeuron.Join(DragNeuron);
        ib.LeftClickNeuron.Join(LeftClickNeuron);
        ib.RightClickNeuron.Join(RightClickNeuron);
        ib.DroppingNeuron.Join(DroppingNeuron);
        ib.DropNeuron.Join(DropNeuron);
        
        ib.DraggingEnterNeuron.Join(DraggingEnterNeuron);
        ib.DraggingExitNeuron.Join(DraggingExitNeuron);
        ib.DraggingMoveNeuron.Join(DraggingMoveNeuron);
    }

    #endregion
}
