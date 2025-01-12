
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] private Button[] Buttons;

    [SerializeField] private Button ReturnButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        EntityBrowser.SetAddress(new Address("Editor.EntityEditableList"));
        EntityBrowser.LeftClickNeuron.Join(SelectEntity);
        EntityBrowser.RightClickNeuron.Join(DeselectEntity);

        SkillBrowser.SetAddress(new Address("SkillInventory"));
        SkillBrowser.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit, CanvasManager.Instance.FormationAnnotation.PointerExit);
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

        UnityAction[] actions = new UnityAction[] { Insert, Remove, MoveUp, MoveDown, Copy, Paste, Save, Load };

        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].onClick.RemoveAllListeners();
            Buttons[i].onClick.AddListener(actions[i]);
        }
        
        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Hide);
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
        CheckAwake();
        Refresh();
    }

    private void SwapTopAndBottom()
    {
        AppManager.Instance.EditorManager.SwapTopAndBottom();
        CheckAwake();
        Refresh();
    }

    private void CopyToBottom()
    {
        AppManager.Instance.EditorManager.CopyToBottom();
        HomeEntityView.SetAddress(new Address("Editor.Home"));
        Refresh();
    }

    private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSkillBarInteractBehaviour))
            return;

        // SkillBarView -> EntityEditorSlotView
        RunSkill skill = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();

        slot.Skill = skill;
        Refresh();
    }

    private void Unequip(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSlotInteractBehaviour))
            return;

        // EntityEditorSlotView -> SkillBarView
        SkillSlot slot = from.Get<SkillSlot>();

        slot.Skill = null;
        Refresh();
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is EntityEditorSlotInteractBehaviour))
            return;

        // EntityEditorSlotView -> EntityEditorSlotView
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();

        (fromSlot.Skill, toSlot.Skill) = (toSlot.Skill, fromSlot.Skill);
        Refresh();
    }

    private void IncreaseJingJie(InteractBehaviour ib, PointerEventData eventData)
    {
        SkillSlot slot = ib.Get<SkillSlot>();
        slot.TryIncreaseJingJie();
        ib.GetView().Refresh();
        RefreshOperationBoard();
        // CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    private void SelectEntity(InteractBehaviour ib, PointerEventData eventData)
        => SelectEntity(ib.GetView().GetBehaviour<SelectBehaviour>());

    private void DeselectEntity(InteractBehaviour ib, PointerEventData eventData)
        => SelectEntity(null);

    private void SelectEntityByIndex(int? selectionIndex)
        => SelectEntity(selectionIndex == null ? null : EntityBrowser.ViewFromIndex(selectionIndex.Value).GetBehaviour<SelectBehaviour>());

    private void SelectEntity(SelectBehaviour selectBehaviour)
    {
        if (_selection != null)
            _selection.SetSelectAsync(false);

        _selection = selectBehaviour;
        _selectionIndex = EntityBrowser.IndexFromView(_selection == null ? null : _selection.GetView());

        // TODO: submit form
        EditorManager.Instance.SetSelectionIndex(_selectionIndex);

        if (_selection != null)
        {
            AwayEntityView.SetAddress(_selection.GetAddress());
            AwayEntityView.Refresh();
            _selection.SetSelectAsync(true);
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
            model.Insert(_selectionIndex.Value, RunEntity.FromTemplate(model[_selectionIndex.Value]));
            EntityBrowser.InsertItem(_selectionIndex.Value);
        }
        else
        {
            model.Add(RunEntity.Default());
            EntityBrowser.AddItem();
        }
    }

    private void Remove()
    {
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        if (!_selectionIndex.HasValue || _selectionIndex.Value >= model.Count())
            return;

        model.RemoveAt(_selectionIndex.Value);
        EntityBrowser.RemoveItemAt(_selectionIndex.Value);
        Refresh();
    }

    private void MoveUp()
    {
        if (_selectionIndex == null)
            return;
        
        if (_selectionIndex == 0)
            return;
        
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        model.Swap(_selectionIndex.Value - 1, _selectionIndex.Value);
        Refresh();
        EntityBrowser.Modified(_selectionIndex.Value);
        SelectEntityByIndex(_selectionIndex.Value - 1);
        EntityBrowser.Modified(_selectionIndex.Value);
    }

    private void MoveDown()
    {
        if (_selectionIndex == null)
            return;
        
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        
        if (_selectionIndex == model.Count() - 1)
            return;
        
        model.Swap(_selectionIndex.Value, _selectionIndex.Value + 1);
        Refresh();
        EntityBrowser.Modified(_selectionIndex.Value);
        SelectEntityByIndex(_selectionIndex.Value + 1);
        EntityBrowser.Modified(_selectionIndex.Value);
    }

    [NonSerialized] private RunEntity _copied;

    private void Copy()
    {
        _copied = RunEntity.FromTemplate(_selection.Get<RunEntity>());
    }

    private void Paste()
    {
        if (_selectionIndex == null)
            return;
        
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        model.RemoveAt(_selectionIndex.Value);
        model.Insert(_selectionIndex.Value, RunEntity.FromTemplate(_copied));
        Refresh();
        EntityBrowser.Modified(_selectionIndex.Value);
    }

    private void Save()
    {
        EditorManager.Instance.Save();
    }

    private void Load()
    {
        EditorManager.Instance.Load();
        CheckAwake();
        EntityBrowser.Sync();
        Refresh();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
