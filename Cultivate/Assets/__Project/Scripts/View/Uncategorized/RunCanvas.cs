
using System;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
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

    public SkillGhost SkillGhost;
    public MechGhost MechGhost;
    [SerializeField] private RunSkillPreview RunSkillPreview;
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
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), Mech */
                /* RunSkill         */ TryMerge,   null,           TryEquipSkill,   null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip,
                /* Mech             */ null,       null,           TryEquipMech,    null,
            });

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
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), Mech */
                /* RunSkill         */ TryMerge,   null,           TryEquipSkill,   null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip,
                /* Mech             */ null,       null,           TryEquipMech,    null,
            },
            lMouseTable: new Func<IInteractable, bool>[]
            {
                ToggleSkill,
                null,
                ToggleSkillSlot,
                null,
            });

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

    public void SetIndexPathForSkillPreview(Address address)
    {
        RunSkillPreview.SetAddress(address);
        RunSkillPreview.Refresh();
    }

    public void UpdateMousePosForSkillPreview(Vector2 pos)
    {
        RunSkillPreview.UpdateMousePos(pos);
        RunSkillPreview.Refresh();
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

    private bool TryMerge(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        RunSkill lhs = from.Get<RunSkill>();
        RunSkill rhs = to.Get<RunSkill>();
        return env.TryMerge(lhs, rhs);
    }

    private bool TryEquipSkill(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        RunSkill toEquip = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();
        return env.TryEquipSkill(toEquip, slot);
    }

    private bool TryEquipMech(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        Mech toEquip = from.Get<Mech>();
        SkillSlot slot = to.Get<SkillSlot>();
        return env.TryEquipMech(toEquip, slot);
    }

    private bool TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        SkillSlot slot = from.Get<SkillSlot>();
        return env.TryUnequip(slot, null);
    }

    private bool TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        return env.TrySwap(fromSlot, toSlot);
    }

    private bool ToggleSkill(IInteractable view)
    {
        return NodeLayer.CardPickerPanel.ToggleSkill(view);
    }

    private bool ToggleSkillSlot(IInteractable view)
    {
        return NodeLayer.CardPickerPanel.ToggleSkillSlot(view);
    }
}
