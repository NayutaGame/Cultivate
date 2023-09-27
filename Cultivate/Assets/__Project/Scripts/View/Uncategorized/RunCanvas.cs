
using System;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunCanvas : Singleton<RunCanvas>
{
    public Color TechColorGreen;
    public Color TechColorYellow;
    public Color TechColorRed;

    public Button BackgroundButton;

    public ReservedLayer ReservedLayer;
    public NodeLayer NodeLayer;
    public MMDMLayer MMDMLayer;

    public TopBar TopBar;

    public Button ToggleShowingConsolePanelButton;
    public ConsolePanel ConsolePanel;

    public MechGhost MechGhost;
    public SkillPreview SkillPreview;
    [SerializeField] private FormationPreview FormationPreview;

    private InteractDelegate DeckInteractDelegate;
    private InteractDelegate CardPickerInteractDelegate;

    public override void DidAwake()
    {
        base.DidAwake();
        Configure();
        Refresh();
    }

    private void ToggleConsolePanel() => ConsolePanel.Toggle();

    public void Configure()
    {
        DeckInteractDelegate = new(4,
            getId: view =>
            {
                if (view is HandSkillView)
                    return 0;
                if (view is SkillInventoryView)
                    return 1;
                if (view is FieldSlotView)
                    return 2;
                if (view is MechView)
                    return 3;
                return null;
            },
            dragDropTable: new Action<IInteractable, IInteractable>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), Mech */
                /* RunSkill         */ TryMerge,   null,           TryEquipSkill,   null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip,
                /* Mech             */ null,       null,           TryEquipMech,    null,
            });

        DeckInteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((HandSkillView)v).HoverAnimation(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((HandSkillView)v).UnhoverAnimation(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((HandSkillView)v).PointerMove(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 0, (v, d) => ((HandSkillView)v).BeginDrag(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.END_DRAG, 0, (v, d) => ((HandSkillView)v).EndDrag(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.DRAG, 0, (v, d) => ((HandSkillView)v).Drag(d));

        DeckInteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 2, (v, d) => ((FieldSlotView)v).HoverAnimation(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 2, (v, d) => ((FieldSlotView)v).UnhoverAnimation(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 2, (v, d) => ((FieldSlotView)v).PointerMove(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 2, (v, d) => ((FieldSlotView)v).BeginDrag(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.END_DRAG, 2, (v, d) => ((FieldSlotView)v).EndDrag(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.DRAG, 2, (v, d) => ((FieldSlotView)v).Drag(d));

        DeckInteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 3, (v, d) => ((MechView)v).BeginDrag(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.END_DRAG, 3, (v, d) => ((MechView)v).EndDrag(d));
        DeckInteractDelegate.SetHandle(InteractDelegate.DRAG, 3, (v, d) => ((MechView)v).Drag(d));

        CardPickerInteractDelegate = new(4,
            getId: view =>
            {
                object item = view.Get<object>();
                if (item is RunSkill)
                    return 0;
                if (item is SkillInventory)
                    return 1;
                if (item is SkillSlot)
                    return 2;
                if (item is Mech)
                    return 3;
                return null;
            },
            dragDropTable: new Action<IInteractable, IInteractable>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), Mech */
                /* RunSkill         */ TryMerge,   null,           TryEquipSkill,   null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip,
                /* Mech             */ null,       null,           TryEquipMech,    null,
            });

        CardPickerInteractDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, ToggleSkill);
        CardPickerInteractDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 2, ToggleSkillSlot);

        BackgroundButton.onClick.RemoveAllListeners();
        BackgroundButton.onClick.AddListener(MMDMLayer.ToggleMap);

        TopBar.Configure();
        ConsolePanel.Configure();

        ToggleShowingConsolePanelButton.onClick.RemoveAllListeners();
        ToggleShowingConsolePanelButton.onClick.AddListener(ToggleConsolePanel);

        ReservedLayer.Configure();
        MMDMLayer.Configure();
        NodeLayer.Configure();

        MMDMLayer.DeckPanel.SetInteractDelegate(DeckInteractDelegate);

        if (!Application.isEditor)
            ConsolePanel.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        TopBar.Refresh();
        ConsolePanel.Refresh();

        ReservedLayer.Refresh();
        NodeLayer.Refresh();
        MMDMLayer.Refresh();
    }

    public void SetIndexPathForSubFormationPreview(Address address)
    {
        FormationPreview.SetAddress(address);
        FormationPreview.Refresh();
    }

    public void UpdateMousePosForSubFormationPreview(Vector2 pos)
    {
        FormationPreview.UpdateMousePos(pos);
        FormationPreview.Refresh();
    }

    public void SetNodeState(PanelDescriptor panelDescriptor)
    {
        if (NodeLayer.CurrentIsDescriptor(panelDescriptor))
        {
            NodeLayer.Refresh();
            return;
        }

        Sequence seq = DOTween.Sequence().SetAutoKill();

        PanelDescriptor d = RunManager.Instance.Battle.Map.CurrentNode?.CurrentPanel;

        if (panelDescriptor == null)
        {
            seq.Join(NodeLayer.SetPanel(panel: null));

            seq.AppendInterval(0.4f)
                .Append(MMDMLayer.SetState(MMDMLayer.MMDMState.MM));
        }
        else
        {
            if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor)
                seq.Append(MMDMLayer.SetState(MMDMLayer.MMDMState.D));
            else
                seq.Append(MMDMLayer.SetState(MMDMLayer.MMDMState.N));

            seq.AppendInterval(0.4f)
                .Join(NodeLayer.SetPanel(d));
        }

        MMDMLayer.DeckPanel.SetInteractDelegate(d is CardPickerPanelDescriptor
            ? CardPickerInteractDelegate
            : DeckInteractDelegate);

        seq.Restart();
    }

    private void TryMerge(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        RunSkill lhs = from.Get<RunSkill>();
        RunSkill rhs = to.Get<RunSkill>();
        env.TryMerge(lhs, rhs);
    }

    private void TryEquipSkill(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        RunSkill toEquip = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();
        env.TryEquipSkill(toEquip, slot);
        MMDMLayer.DeckPanel.FieldView.Refresh();
    }

    private void TryEquipMech(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        Mech toEquip = from.Get<Mech>();
        SkillSlot slot = to.Get<SkillSlot>();
        env.TryEquipMech(toEquip, slot);
        from.Refresh();
        MMDMLayer.DeckPanel.FieldView.Refresh();
    }

    private void TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        SkillSlot slot = from.Get<SkillSlot>();
        env.TryUnequip(slot, null);
        MMDMLayer.DeckPanel.FieldView.Refresh();
    }

    private void TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        env.TrySwap(fromSlot, toSlot);
        MMDMLayer.DeckPanel.FieldView.Refresh();
    }

    private void ToggleSkill(IInteractable view, PointerEventData eventData)
    {
        NodeLayer.CardPickerPanel.ToggleSkill(view);
        // RunCanvas.Instance.Refresh();
    }

    private void ToggleSkillSlot(IInteractable view, PointerEventData eventData)
    {
        NodeLayer.CardPickerPanel.ToggleSkillSlot(view);
        // RunCanvas.Instance.Refresh();
    }
}
