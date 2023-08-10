
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
                object item = DataManager.Get<object>(view.GetIndexPath());
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

        CardPickerInteractDelegate = new(3,
            getId: view =>
            {
                object item = DataManager.Get<object>(view.GetIndexPath());
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

    public void SetIndexPathForSkillPreview(IndexPath indexPath)
    {
        RunSkillPreview.Configure(indexPath);
        RunSkillPreview.Refresh();
    }

    public void UpdateMousePosForSkillPreview(Vector2 pos)
    {
        RunSkillPreview.UpdateMousePos(pos);
        RunSkillPreview.Refresh();
    }

    public void SetIndexPathForSubFormationPreview(IndexPath indexPath)
    {
        FormationPreview.Configure(indexPath);
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
            return;

        Sequence seq = DOTween.Sequence().SetAutoKill();

        PanelDescriptor d = RunManager.Instance.TryGetCurrentNode()?.CurrentPanel;

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
        RunEnvironment env = DataManager.Get<RunEnvironment>(new IndexPath("Run.Battle"));
        RunSkill lhs = DataManager.Get<RunSkill>(from.GetIndexPath());
        RunSkill rhs = DataManager.Get<RunSkill>(to.GetIndexPath());
        return env.TryMerge(lhs, rhs);
    }

    private bool TryEquipSkill(IInteractable from, IInteractable to)
    {
        RunEnvironment env = DataManager.Get<RunEnvironment>(new IndexPath("Run.Battle"));
        RunSkill toEquip = DataManager.Get<RunSkill>(from.GetIndexPath());
        SkillSlot slot = DataManager.Get<SkillSlot>(to.GetIndexPath());
        return env.TryEquipSkill(toEquip, slot);
    }

    private bool TryEquipMech(IInteractable from, IInteractable to)
    {
        RunEnvironment env = DataManager.Get<RunEnvironment>(new IndexPath("Run.Battle"));
        Mech toEquip = DataManager.Get<Mech>(from.GetIndexPath());
        SkillSlot slot = DataManager.Get<SkillSlot>(to.GetIndexPath());
        return env.TryEquipMech(toEquip, slot);
    }

    private bool TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment env = DataManager.Get<RunEnvironment>(new IndexPath("Run.Battle"));
        SkillSlot slot = DataManager.Get<SkillSlot>(from.GetIndexPath());
        return env.TryUnequip(slot, null);
    }

    private bool TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment env = DataManager.Get<RunEnvironment>(new IndexPath("Run.Battle"));
        SkillSlot fromSlot = DataManager.Get<SkillSlot>(from.GetIndexPath());
        SkillSlot toSlot = DataManager.Get<SkillSlot>(to.GetIndexPath());
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
