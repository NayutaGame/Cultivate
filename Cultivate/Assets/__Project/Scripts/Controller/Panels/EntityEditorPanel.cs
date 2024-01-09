
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorPanel : Panel
{
    [SerializeField] private ListView EntityBrowser;
    private int? _selectionIndex;
    private SelectBehaviour _selection;

    [SerializeField] private ListView SkillBrowser;

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

    public override void Configure()
    {
        base.Configure();

        EntityBrowser.SetAddress(new Address("Editor.EntityEditableList"));
        EntityBrowser.LeftClickNeuron.Join(SelectEntity);

        SkillBrowser.SetAddress(new Address("SkillInventory"));
        SkillBrowser.PointerEnterNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressFromIB);
        SkillBrowser.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        SkillBrowser.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
        SkillBrowser.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull,
            CanvasManager.Instance.FormationAnnotation.SetAddressToNull);

        AwayEntityView.SetAddress(null);
        AwayEntityView.PointerEnterSlotNeuron.Join(ShowSkillAnnotationFromSlotView);
        AwayEntityView.PointerExitSlotNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        AwayEntityView.PointerMoveSlotNeuron.Join(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
        AwayEntityView.BeginDragSlotNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull,
            CanvasManager.Instance.FormationAnnotation.SetAddressToNull);

        AwayEntityView.RightClickSlotNeuron.Join(IncreaseJingJie);
        AwayEntityView.PointerEnterFormationNeuron.Join(CanvasManager.Instance.FormationAnnotation.SetAddressFromIB);
        AwayEntityView.PointerExitFormationNeuron.Join(CanvasManager.Instance.FormationAnnotation.SetAddressToNull);
        AwayEntityView.PointerMoveFormationNeuron.Join(CanvasManager.Instance.FormationAnnotation.UpdateMousePos);

        HomeEntityView.SetAddress(new Address("Editor.Home"));
        HomeEntityView.PointerEnterSlotNeuron.Join(ShowSkillAnnotationFromSlotView);
        HomeEntityView.PointerExitSlotNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        HomeEntityView.PointerMoveSlotNeuron.Join(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
        HomeEntityView.BeginDragSlotNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull,
            CanvasManager.Instance.FormationAnnotation.SetAddressToNull);

        HomeEntityView.RightClickSlotNeuron.Join(IncreaseJingJie);
        HomeEntityView.PointerEnterFormationNeuron.Join(CanvasManager.Instance.FormationAnnotation.SetAddressFromIB);
        HomeEntityView.PointerExitFormationNeuron.Join(CanvasManager.Instance.FormationAnnotation.SetAddressToNull);
        HomeEntityView.PointerMoveFormationNeuron.Join(CanvasManager.Instance.FormationAnnotation.UpdateMousePos);

        AwayEntityView.DropSlotNeuron.Join(Equip, Swap);

        HomeEntityView.DropSlotNeuron.Join(Equip, Swap);

        SkillBrowser.DropNeuron.Join(Unequip);

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

    private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSkillBarInteractBehaviour))
            return;

        // SkillBarView -> EntityEditorSlotView
        RunSkill skill = from.ComplexView.Get<RunSkill>();
        SkillSlot slot = to.ComplexView.Get<SkillSlot>();

        slot.Skill = skill;
        Refresh();
    }

    private void Unequip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSlotInteractBehaviour))
            return;

        // EntityEditorSlotView -> SkillBarView
        SkillSlot slot = from.ComplexView.Get<SkillSlot>();

        slot.Skill = null;
        Refresh();
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSlotInteractBehaviour))
            return;

        // EntityEditorSlotView -> EntityEditorSlotView
        SkillSlot fromSlot = from.ComplexView.Get<SkillSlot>();
        SkillSlot toSlot = to.ComplexView.Get<SkillSlot>();

        (fromSlot.Skill, toSlot.Skill) = (toSlot.Skill, fromSlot.Skill);
        Refresh();
    }

    private void IncreaseJingJie(InteractBehaviour ib, PointerEventData eventData)
    {
        SkillSlot slot = ib.ComplexView.Get<SkillSlot>();
        slot.TryIncreaseJingJie();
        ib.ComplexView.Refresh();
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    private void ShowSkillAnnotationFromSlotView(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.SkillAnnotation.SetAddress(ib.ComplexView.GetAddress().Append(".Skill"));
    }

    private void SelectEntity(InteractBehaviour ib, PointerEventData eventData)
        => SelectEntity(ib.ComplexView.GetSelectBehaviour());

    private void SelectEntity(SelectBehaviour selectBehaviour)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = selectBehaviour;
        _selectionIndex = EntityBrowser.IndexFromItemBehaviour(_selection.ComplexView.GetItemBehaviour());

        // TODO: submit form
        EditorManager.Instance._selectionIndex = _selectionIndex;

        if (_selection != null)
        {
            AwayEntityView.SetAddress(_selection.ComplexView.GetAddress());
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
