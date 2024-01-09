
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddressBehaviour : MonoBehaviour
{
    public RectTransform Base;

    public List<HoverStrategy> HoverStrategies;
    public List<DragStrategy> DragStrategies;
    public List<DropStrategy> DropStrategies;
    public List<SelectStrategy> SelectStrategies;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private bool _inited = false;

    public void Init()
    {
        _inited = true;
        HoverStrategies = new();
        DragStrategies = new();
        DropStrategies = new();
        SelectStrategies = new();

        HoverStrategies.Add(new HoverStrategyPivot(null, null, null));
        DragStrategies.Add(new DragStrategyGhost(CanvasManager.Instance.SkillGhost));

        foreach (var hover in HoverStrategies)
        {
            PointerEnterNeuron.Add(hover.AnimateStateToHover);
            PointerExitNeuron.Add(hover.AnimateStateToIdle);
        }

        if (DragStrategies.Count != 0)
        {
            BeginDragNeuron.Add(CanvasManager.Instance.ClearAnnotations);
            BeginDragNeuron.Add(SetInteractableToFalse);
            BeginDragNeuron.Add(SetVisibleToFalse);
            EndDragNeuron.Add(SetInteractableToTrue);
            EndDragNeuron.Add(SetVisibleToTrue);
        }

        foreach (var drag in DragStrategies)
        {
            BeginDragNeuron.Add(drag.BeginDrag);
            EndDragNeuron.Add(drag.EndDrag);
            DragNeuron.Add(drag.Drag);
        }

        // loaders
    }

    private void OnEnable()
    {
        InteractBehaviour ib = GetComponent<InteractBehaviour>();
        ib.PointerEnterNeuron.Add(PointerEnterNeuron);
        ib.PointerExitNeuron.Add(PointerExitNeuron);
        ib.PointerMoveNeuron.Add(PointerMoveNeuron);
        ib.BeginDragNeuron.Add(BeginDragNeuron);
        ib.EndDragNeuron.Add(EndDragNeuron);
        ib.DragNeuron.Add(DragNeuron);
        ib.LeftClickNeuron.Add(LeftClickNeuron);
        ib.RightClickNeuron.Add(RightClickNeuron);
        ib.DropNeuron.Add(DropNeuron);
    }

    private void OnDisable()
    {
        InteractBehaviour ib = GetComponent<InteractBehaviour>();
        ib.PointerEnterNeuron.Remove(PointerEnterNeuron);
        ib.PointerExitNeuron.Remove(PointerExitNeuron);
        ib.PointerMoveNeuron.Remove(PointerMoveNeuron);
        ib.BeginDragNeuron.Remove(BeginDragNeuron);
        ib.EndDragNeuron.Remove(EndDragNeuron);
        ib.DragNeuron.Remove(DragNeuron);
        ib.LeftClickNeuron.Remove(LeftClickNeuron);
        ib.RightClickNeuron.Remove(RightClickNeuron);
        ib.DropNeuron.Remove(DropNeuron);
    }

    public void TryInit()
    {
        if (!_inited)
            Init();
    }

    public virtual void SetAddress(Address address)
    {
        TryInit();
        _address = address;
    }

    public virtual void Refresh()
    {
    }

    private bool _visible;
    public bool IsVisible() => _visible;
    public void SetVisible(bool visible)
    {
        _visible = visible;

        // AddressBehaviour.SetVisible(_visible);
    }

    public void SetVisibleToTrue(MonoBehaviour behaviour, PointerEventData eventData)
        => SetVisible(true);

    public void SetVisibleToFalse(MonoBehaviour behaviour, PointerEventData eventData)
        => SetVisible(false);

    private bool _interactable;
    public virtual bool IsInteractable() => _interactable;
    public virtual void SetInteractable(bool interactable)
    {
        _interactable = interactable;

        // if (InteractBehaviour != null)
        //     InteractBehaviour.SetInteractable(_interactable);
    }

    public void SetInteractableToTrue(MonoBehaviour behaviour, PointerEventData eventData)
        => SetInteractable(true);

    public void SetInteractableToFalse(MonoBehaviour behaviour, PointerEventData eventData)
        => SetInteractable(false);

    // [SerializeField] private CanvasGroup CanvasGroup;
    //
    // public void SetVisible(bool visible)
    // {
    //     CanvasGroup.alpha = visible ? 1 : 0;
    // }

    #region Neurons

    public Neuron<AddressBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<AddressBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<AddressBehaviour, AddressBehaviour, PointerEventData> DropNeuron = new();

    #endregion
}
