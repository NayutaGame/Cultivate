
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorPanel : Panel
{
    [SerializeField] private ListView EntityBrowser;
    private int? _selectionIndex;
    private EntityBarView _selection;

    [SerializeField] private ListView SkillBrowser;
    [SerializeField] private SkillAnnotation SkillAnnotation;
    [SerializeField] private FormationPreview FormationPreview;

    [SerializeField] private EntityEditorEntityView AwayEntityView;
    [SerializeField] private EntityEditorEntityView HomeEntityView;
    [SerializeField] private Button CopyToTopButton;
    [SerializeField] private Button SwapTopAndBottomButton;
    [SerializeField] private Button CopyToBottomButton;

    [SerializeField] private TMP_Text Result;
    [SerializeField] private Button CombatButton;

    [SerializeField] private Button InsertButton;
    [SerializeField] private Button RemoveButton;
    [SerializeField] private Button SaveButton;
    [SerializeField] private Button LoadButton;

    private InteractHandler _interactHandler;

    public override void Configure()
    {
        base.Configure();

        EntityBrowser.SetAddress(new Address("Editor.EntityEditableList"));
        SkillBrowser.SetAddress(new Address("SkillInventory"));
        AwayEntityView.SetAddress(null);
        HomeEntityView.SetAddress(new Address("Editor.Home"));

        ConfigureInteractDelegate();

        CopyToTopButton.onClick.RemoveAllListeners();
        CopyToTopButton.onClick.AddListener(CopyToTop);
        SwapTopAndBottomButton.onClick.RemoveAllListeners();
        SwapTopAndBottomButton.onClick.AddListener(SwapTopAndBottom);
        CopyToBottomButton.onClick.RemoveAllListeners();
        CopyToBottomButton.onClick.AddListener(CopyToBottom);

        CombatButton.onClick.RemoveAllListeners();
        CombatButton.onClick.AddListener(Combat);

        InsertButton.onClick.RemoveAllListeners();
        InsertButton.onClick.AddListener(Insert);

        RemoveButton.onClick.RemoveAllListeners();
        RemoveButton.onClick.AddListener(Remove);

        SaveButton.onClick.RemoveAllListeners();
        SaveButton.onClick.AddListener(Save);

        LoadButton.onClick.RemoveAllListeners();
        LoadButton.onClick.AddListener(Load);
    }

    private void CopyToTop()
    {
        AppManager.Instance.EditorManager.CopyToTop();
        HomeEntityView.Refresh();
        AwayEntityView.Refresh();
    }

    private void SwapTopAndBottom()
    {
        AppManager.Instance.EditorManager.SwapTopAndBottom();
        HomeEntityView.Refresh();
        AwayEntityView.Refresh();
    }

    private void CopyToBottom()
    {
        AppManager.Instance.EditorManager.CopyToBottom();
        HomeEntityView.Refresh();
        AwayEntityView.Refresh();
    }

    private void ConfigureInteractDelegate()
    {
        _interactHandler = new(4,
            getId: view =>
            {
                InteractBehaviour d = view.GetComponent<InteractBehaviour>();
                if (d is EntityBarInteractBehaviour)
                    return 0;
                if (d is EntityEditorSkillBarInteractBehaviour)
                    return 1;
                if (view is EntityEditorSlotView)
                    return 2;
                if (d is RunFormationIconInteractBehaviour)
                    return 3;
                return null;
            });

        _interactHandler.SetDragDrop(1, 2, Equip);
        _interactHandler.SetDragDrop(2, 1, Unequip);
        _interactHandler.SetDragDrop(2, 2, Swap);

        _interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, SelectEntity);

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 1, ShowSkillPreview);
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 1, HideSkillPreview);
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 1, MoveSkillPreview);
        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 1, BeginDrag);

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 2, ShowSkillPreviewFromSlotView);
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 2, HideSkillPreview);
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 2, MoveSkillPreview);
        _interactHandler.SetHandle(InteractHandler.BEGIN_DRAG, 2, BeginDrag);

        _interactHandler.SetHandle(InteractHandler.POINTER_RIGHT_CLICK, 2, IncreaseJingJie);

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 3, ShowFormationPreview);
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 3, HideFormationPreview);
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 3, MoveFormationPreview);

        EntityBrowser.SetHandler(_interactHandler);
        SkillBrowser.SetHandler(_interactHandler);
        AwayEntityView.SetHandler(_interactHandler);
        HomeEntityView.SetHandler(_interactHandler);
    }

    private void Equip(InteractBehaviour fromInteractBehaviour, InteractBehaviour toInteractBehaviour)
    {
        // SkillBarView -> EntityEditorSlotView
        RunSkill skill = fromInteractBehaviour.AddressBehaviour.Get<RunSkill>();
        SkillSlot slot = toInteractBehaviour.AddressBehaviour.Get<SkillSlot>();

        slot.Skill = skill;
        Refresh();
    }

    private void Unequip(InteractBehaviour fromInteractBehaviour, InteractBehaviour toInteractBehaviour)
    {
        // EntityEditorSlotView -> SkillBarView
        SkillSlot slot = fromInteractBehaviour.AddressBehaviour.Get<SkillSlot>();

        slot.Skill = null;
        Refresh();
    }

    private void Swap(InteractBehaviour fromInteractBehaviour, InteractBehaviour toInteractBehaviour)
    {
        // EntityEditorSlotView -> EntityEditorSlotView
        SkillSlot fromSlot = fromInteractBehaviour.AddressBehaviour.Get<SkillSlot>();
        SkillSlot toSlot = toInteractBehaviour.AddressBehaviour.Get<SkillSlot>();

        (fromSlot.Skill, toSlot.Skill) = (toSlot.Skill, fromSlot.Skill);
        Refresh();
    }

    private void IncreaseJingJie(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        SkillSlot slot = interactBehaviour.AddressBehaviour.Get<SkillSlot>();
        slot.TryIncreaseJingJie();
        interactBehaviour.AddressBehaviour.Refresh();
        SkillAnnotation.Refresh();
    }

    private void ShowSkillPreview(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillAnnotation.SetAddress(interactBehaviour.AddressBehaviour.GetAddress());
        SkillAnnotation.Refresh();
    }

    private void ShowSkillPreviewFromSlotView(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillAnnotation.SetAddress(interactBehaviour.AddressBehaviour.GetAddress().Append(".Skill"));
        SkillAnnotation.Refresh();
    }

    private void HideSkillPreview(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillAnnotation.SetAddress(null);
        SkillAnnotation.Refresh();
    }

    private void MoveSkillPreview(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillAnnotation.UpdateMousePos(eventData.position);
    }

    private void ShowFormationPreview(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        FormationPreview.SetAddress(interactBehaviour.AddressBehaviour.GetAddress());
        FormationPreview.Refresh();
    }

    private void HideFormationPreview(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        FormationPreview.SetAddress(null);
        FormationPreview.Refresh();
    }

    private void MoveFormationPreview(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        FormationPreview.UpdateMousePos(eventData.position);
    }

    private void BeginDrag(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        SkillAnnotation.SetAddress(null);
        SkillAnnotation.Refresh();
        FormationPreview.SetAddress(null);
        FormationPreview.Refresh();
    }

    private void SelectEntity(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = interactBehaviour.GetComponent<EntityBarView>();

        if (_selection != null)
            _selectionIndex = EntityBrowser.ActivePool.IndexOf(_selection);
        else
            _selectionIndex = null;

        EditorManager.Instance._selectionIndex = _selectionIndex;

        if (_selection != null)
        {
            AwayEntityView.SetAddress(interactBehaviour.AddressBehaviour.GetAddress());
            AwayEntityView.Refresh();
            _selection.SetSelected(true);
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        AwayEntityView.Refresh();
        HomeEntityView.Refresh();

        if (EditorManager.Instance.SimulateResult is { } result)
        {
            Result.text = $"玩家 : 怪物\n{result.HomeLeftHp} : {result.AwayLeftHp}";
            Result.color = result.HomeVictory ? Color.green : Color.red;
        }
        else
        {
            Result.text = $"玩家 : 怪物\n无结果";
            Result.color = Color.gray;
        }
    }

    private void Combat()
    {
        EditorManager.Instance.Combat();
    }

    private void Insert()
    {
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        if (_selectionIndex.HasValue)
        {
            model.Insert(_selectionIndex.Value, RunEntity.Default());
        }
        else
        {
            model.Add(RunEntity.Default());
        }
    }

    private void Remove()
    {
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        if (!_selectionIndex.HasValue || _selectionIndex.Value >= model.Count())
            return;

        model.RemoveAt(_selectionIndex.Value);
    }

    private void Save()
    {
        EditorManager.Instance.Save();
    }

    private void Load()
    {
        EditorManager.Instance.Load();
        Configure();
    }

    // private bool TryMerge(IInteractable from, IInteractable to)
    // {
    //     RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
    //     RunSkill lhs = from.Get<RunSkill>();
    //     RunSkill rhs = to.Get<RunSkill>();
    //     return runEnvironment.TryMerge(lhs, rhs);
    // }
    //
    // private bool TryEquip(IInteractable from, IInteractable to)
    // {
    //     RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
    //     RunSkill toEquip = from.Get<RunSkill>();
    //     SkillSlot slot = to.Get<SkillSlot>();
    //     return runEnvironment.TryEquipSkill(toEquip, slot);
    // }
    //
    // private bool TryUnequip(IInteractable from, IInteractable to)
    // {
    //     RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
    //     SkillSlot slot = from.Get<SkillSlot>();
    //     return runEnvironment.TryUnequip(slot, null);
    // }
    //
    // private bool TrySwap(IInteractable from, IInteractable to)
    // {
    //     RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
    //     SkillSlot fromSlot = from.Get<SkillSlot>();
    //     SkillSlot toSlot = to.Get<SkillSlot>();
    //     return runEnvironment.TrySwap(fromSlot, toSlot);
    // }
    //
    // private bool TryWrite(IInteractable from, IInteractable to)
    // {
    //     RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
    //
    //     object fromItem = from.Get<object>();
    //     SkillSlot toSlot = to.Get<SkillSlot>();
    //
    //     if (fromItem is RunSkill fromSkill)
    //     {
    //         return runEnvironment.TryWrite(fromSkill, toSlot);
    //     }
    //     else if (fromItem is SkillSlot fromSlot)
    //     {
    //         return runEnvironment.TryWrite(fromSlot, toSlot);
    //     }
    //
    //     return false;
    // }
    //
    // private bool TryIncreaseJingJie(IInteractable view)
    // {
    //     RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
    //
    //     object item = view.Get<object>();
    //
    //     if (item is RunSkill skill)
    //     {
    //         return runEnvironment.TryIncreaseJingJie(skill);
    //     }
    //     else if (item is SkillSlot slot)
    //     {
    //         return runEnvironment.TryIncreaseJingJie(slot);
    //     }
    //
    //     return false;
    // }
}
