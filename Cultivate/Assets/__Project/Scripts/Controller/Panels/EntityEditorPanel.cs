
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorPanel : Panel
{
    [SerializeField] private LegacyListView EntityBrowser;
    private int? _selectionIndex;
    private LegacySelectBehaviour _selection;

    [SerializeField] private LegacyListView SkillBrowser;

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
        EntityBrowser.RightClickNeuron.Join(DeselectEntity);

        SkillBrowser.SetAddress(new Address("SkillInventory"));
        SkillBrowser.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit,
            CanvasManager.Instance.FormationAnnotation.PointerExit);
        SkillBrowser.DropNeuron.Join(Unequip);

        AwayEntityView.SetAddress(null);
        AwayEntityView.RightClickSlotNeuron.Join(IncreaseJingJie);
        AwayEntityView.DropSlotNeuron.Join(Equip, Swap);
        AwayEntityView.DropSmirkAgainstSlotNeuron.Join(Equip, Swap);
        AwayEntityView.DropAfraidAgainstSlotNeuron.Join(Equip, Swap);

        HomeEntityView.SetAddress(new Address("Editor.Home"));
        HomeEntityView.RightClickSlotNeuron.Join(IncreaseJingJie);
        HomeEntityView.DropSlotNeuron.Join(Equip, Swap);
        HomeEntityView.DropSmirkAgainstSlotNeuron.Join(Equip, Swap);
        HomeEntityView.DropAfraidAgainstSlotNeuron.Join(Equip, Swap);
        
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

    public override void Refresh()
    {
        base.Refresh();
        AwayEntityView.Refresh();
        RefreshOperationBoard();
        HomeEntityView.Refresh();
    }

    private void CopyToTop()
    {
        AppManager.Instance.EditorManager.CopyToTop();
        Configure();
        Refresh();
    }

    private void SwapTopAndBottom()
    {
        AppManager.Instance.EditorManager.SwapTopAndBottom();
        Configure();
        Refresh();
    }

    private void CopyToBottom()
    {
        AppManager.Instance.EditorManager.CopyToBottom();
        HomeEntityView.SetAddress(new Address("Editor.Home"));
        Refresh();
    }

    private void Equip(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSkillBarInteractBehaviour))
            return;

        // SkillBarView -> EntityEditorSlotView
        RunSkill skill = from.GetSimpleView().Get<RunSkill>();
        SkillSlot slot = to.GetSimpleView().Get<SkillSlot>();

        slot.Skill = skill;
        Refresh();
    }

    private void Unequip(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSlotInteractBehaviour))
            return;

        // EntityEditorSlotView -> SkillBarView
        SkillSlot slot = from.GetSimpleView().Get<SkillSlot>();

        slot.Skill = null;
        Refresh();
    }

    private void Swap(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSlotInteractBehaviour))
            return;

        // EntityEditorSlotView -> EntityEditorSlotView
        SkillSlot fromSlot = from.GetSimpleView().Get<SkillSlot>();
        SkillSlot toSlot = to.GetSimpleView().Get<SkillSlot>();

        (fromSlot.Skill, toSlot.Skill) = (toSlot.Skill, fromSlot.Skill);
        Refresh();
    }

    private void IncreaseJingJie(LegacyInteractBehaviour ib, PointerEventData eventData)
    {
        SkillSlot slot = ib.GetSimpleView().Get<SkillSlot>();
        slot.TryIncreaseJingJie();
        ib.GetSimpleView().Refresh();
        RefreshOperationBoard();
        // CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    private void SelectEntity(LegacyInteractBehaviour ib, PointerEventData eventData)
        => SelectEntity(ib.GetSimpleView().GetSelectBehaviour());

    private void DeselectEntity(LegacyInteractBehaviour ib, PointerEventData eventData)
        => SelectEntity(null);

    private void SelectEntity(LegacySelectBehaviour selectBehaviour)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = selectBehaviour;
        _selectionIndex = EntityBrowser.IndexFromItemBehaviour(_selection == null ? null : _selection.GetSimpleView().GetItemBehaviour());

        // TODO: submit form
        EditorManager.Instance.SetSelectionIndex(_selectionIndex);

        if (_selection != null)
        {
            AwayEntityView.SetAddress(_selection.GetSimpleView().GetAddress());
            AwayEntityView.Refresh();
            _selection.SetSelected(true);
        }
        
        RefreshOperationBoard();
    }

    private void RefreshOperationBoard()
    {
        if (EditorManager.Instance.SimulateResult is { } result)
        {
            Result.text =
                $@"
客场    {result.AwayLeftHp}
——————
主场    {result.HomeLeftHp}"
                ;
            Result.color = result.Flag == 1 ? Color.green : Color.red;
        }
        else
        {
            Result.text =
            $@"
客场    无结果
——————
主场    无结果"
                ;
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
