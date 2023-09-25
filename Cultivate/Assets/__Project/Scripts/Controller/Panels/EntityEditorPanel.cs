
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorPanel : Panel
{
    public Button CreateNewEntityButton;
    public ListView EntityBrowser;
    private int? _selectionIndex;
    private EntityBarView _selection;

    public ListView SkillBrowser;
    public SkillPreview SkillPreview;

    public EntityEditorEntityView AwayEntityView;
    public EntityEditorEntityView HomeEntityView;
    public Button CopyToTopButton;
    public Button SwapTopAndBottomButton;
    public Button CopyToBottomButton;

    private InteractDelegate InteractDelegate;

    public override void Configure()
    {
        base.Configure();

        CreateNewEntityButton.onClick.RemoveAllListeners();
        CreateNewEntityButton.onClick.AddListener(CreateNewEntity);
        EntityBrowser.SetAddress(new Address("Encyclopedia.EntityEditableList"));
        SkillBrowser.SetAddress(new Address("SkillInventory"));
        AwayEntityView.SetAddress(null);
        HomeEntityView.SetAddress(new Address("Encyclopedia.EntityEditorHomeEntity"));

        ConfigureInteractDelegate();

        CopyToTopButton.onClick.RemoveAllListeners();
        CopyToTopButton.onClick.AddListener(CopyToTop);
        SwapTopAndBottomButton.onClick.RemoveAllListeners();
        SwapTopAndBottomButton.onClick.AddListener(SwapTopAndBottom);
        CopyToBottomButton.onClick.RemoveAllListeners();
        CopyToBottomButton.onClick.AddListener(CopyToBottom);

        // ReportButton.onClick.RemoveAllListeners();
        // ReportButton.onClick.AddListener(Report);
        //
        // StreamButton.onClick.RemoveAllListeners();
        // StreamButton.onClick.AddListener(Stream);
    }

    private void CreateNewEntity()
    {
        ListModel<RunEntity> model = EntityBrowser.Get<ListModel<RunEntity>>();
        model.Add(RunEntity.Default);
    }

    private void CopyToTop()
    {
        if (_selectionIndex == null)
            return;

        Encyclopedia.Instance.CopyToTop(_selectionIndex.Value);
        HomeEntityView.Refresh();
        AwayEntityView.Refresh();
    }

    private void SwapTopAndBottom()
    {
        if (_selectionIndex == null)
            return;

        Encyclopedia.Instance.SwapTopAndBottom(_selectionIndex.Value);
        HomeEntityView.Refresh();
        AwayEntityView.Refresh();
    }

    private void CopyToBottom()
    {
        if (_selectionIndex == null)
            return;

        Encyclopedia.Instance.CopyToBottom(_selectionIndex.Value);
        HomeEntityView.Refresh();
        AwayEntityView.Refresh();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(3,
            getId: view =>
            {
                if (view is EntityBarView)
                    return 0;
                if (view is EntityEditorSkillBarView)
                    return 1;
                if (view is EntityEditorSlotView)
                    return 2;
                return null;
            },
            dragDropTable: new Action<IInteractable, IInteractable>[]
            {
                /*                         EntityBarView, SkillBarView, EntityEditorSlotView */
                /* EntityBarView        */ null,          null,         null,
                /* SkillBarView         */ null,          null,         Equip,
                /* EntityEditorSlotView */ null,          Unequip,      Swap,
            });

        InteractDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, SelectEntity);

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 1, ShowPreview);
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 1, HidePreview);
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 1, MovePreview);
        InteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 1, BeginDrag);

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 2, ShowPreviewFromSlotView);
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 2, HidePreview);
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 2, MovePreview);
        InteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 2, BeginDrag);

        InteractDelegate.SetHandle(InteractDelegate.POINTER_RIGHT_CLICK, 2, IncreaseJingJie);

        EntityBrowser.SetDelegate(InteractDelegate);
        SkillBrowser.SetDelegate(InteractDelegate);
        AwayEntityView.SetDelegate(InteractDelegate);
        HomeEntityView.SetDelegate(InteractDelegate);
    }

    private void Equip(IInteractable fromView, IInteractable toView)
    {
        // SkillBarView -> EntityEditorSlotView
        RunSkill skill = fromView.Get<RunSkill>();
        SkillSlot slot = toView.Get<SkillSlot>();

        slot.Skill = skill;
        toView.Refresh();
    }

    private void Unequip(IInteractable fromView, IInteractable toView)
    {
        // EntityEditorSlotView -> SkillBarView
        SkillSlot slot = fromView.Get<SkillSlot>();

        slot.Skill = null;
        fromView.Refresh();
    }

    private void Swap(IInteractable fromView, IInteractable toView)
    {
        // EntityEditorSlotView -> EntityEditorSlotView
        SkillSlot fromSlot = fromView.Get<SkillSlot>();
        SkillSlot toSlot = toView.Get<SkillSlot>();

        (fromSlot.Skill, toSlot.Skill) = (toSlot.Skill, fromSlot.Skill);
        fromView.Refresh();
        toView.Refresh();
    }

    private void IncreaseJingJie(IInteractable view, PointerEventData eventData)
    {
        SkillSlot slot = view.Get<SkillSlot>();
        slot.TryIncreaseJingJie();
        view.Refresh();
        SkillPreview.Refresh();
    }

    private void ShowPreview(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillPreview.SetAddress(view.GetAddress());
        SkillPreview.Refresh();
    }

    private void ShowPreviewFromSlotView(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillPreview.SetAddress(view.GetAddress().Append(".Skill"));
        SkillPreview.Refresh();
    }

    private void HidePreview(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillPreview.SetAddress(null);
        SkillPreview.Refresh();
    }

    private void MovePreview(IInteractable view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SkillPreview.UpdateMousePos(eventData.position);
    }

    private void BeginDrag(IInteractable view, PointerEventData eventData)
    {
        SkillPreview.SetAddress(null);
        SkillPreview.Refresh();
    }

    private void SelectEntity(IInteractable view, PointerEventData eventData)
    {
        if (_selection != null)
            _selection.SetSelected(false);

        _selection = (EntityBarView)view;

        if (_selection != null)
            _selectionIndex = EntityBrowser.ActivePool.IndexOf(_selection);
        else
            _selectionIndex = null;

        if (_selection != null)
        {
            AwayEntityView.SetAddress(view.GetAddress());
            AwayEntityView.Refresh();
            _selection.SetSelected(true);
        }
    }

    public override void Refresh()
    {
        // HeroView.Refresh();
        // EnemyView.Refresh();
        // SkillInventoryView.Refresh();

        // if (RunManager.Instance.Simulate.Report is { } report)
        // {
        //     SimulatedHP.text = $"玩家 : 怪物\n{report.HomeLeftHp} : {report.AwayLeftHp}";
        //     Light.color = report.HomeVictory ? Color.green : Color.red;
        //     ReportText.text = report.ToString();
        // }
        // else
        // {
        //     SimulatedHP.text = $"玩家 : 怪物\n无结果";
        //     Light.color = Color.gray;
        //     ReportText.text = "";
        // }
    }

    private void Report()
    {
        // RunManager.Instance.Combat(false, RunManager.Instance.Simulate);
        // RunCanvas.Instance.Refresh();
    }

    private void Stream()
    {
        // RunManager.Instance.Combat(true, RunManager.Instance.Simulate);
        // RunCanvas.Instance.Refresh();
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
