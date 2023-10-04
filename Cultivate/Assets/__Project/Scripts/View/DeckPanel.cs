
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeckPanel : Panel
{
    public Image ToggleImage;
    public Button ToggleButton;
    public Button SortButton;

    public ListView FieldView;
    public SkillInventoryView HandView;
    public ListView FormationListView;
    public ListView MechListView;

    public RectTransform _deckTransform;
    public RectTransform _spriteTransform;
    public RectTransform _handTransform;
    public RectTransform _subFormationInventoryTransform;
    public RectTransform _mechBagTransform;

    public override Tween GetShowTween()
        => DOTween.Sequence()
            .AppendCallback(HandView.BigRefresh)
            .Join(_deckTransform.                  DOAnchorPosY(-69.5f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0f))
            .Join(_spriteTransform.                DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.03f))
            .Join(_handTransform.                  DOAnchorPosY(94f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.06f))
            .Join(_subFormationInventoryTransform. DOAnchorPosY(200f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.09f))
            .Join(_mechBagTransform.               DOAnchorPosY(-255f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.12f));

    public override Tween GetHideTween()
        => DOTween.Sequence()
            .Join(_deckTransform.                  DOAnchorPosY(-445, 0.3f).SetEase(Ease.InQuad).SetDelay(0f))
            .Join(_spriteTransform.                DOAnchorPosY(-626, 0.3f).SetEase(Ease.InQuad).SetDelay(0.03f))
            .Join(_handTransform.                  DOAnchorPosY(-403, 0.3f).SetEase(Ease.InQuad).SetDelay(0.06f))
            .Join(_subFormationInventoryTransform. DOAnchorPosY(-364, 0.3f).SetEase(Ease.InQuad).SetDelay(0.09f))
            .Join(_mechBagTransform.               DOAnchorPosY(-375, 0.3f).SetEase(Ease.InQuad).SetDelay(0.12f));

    public override void Configure()
    {
        FieldView.SetAddress(new Address("Run.Battle.Hero.Slots"));
        HandView.SetAddress(new Address("Run.Battle.SkillInventory"));
        FormationListView.SetAddress(new Address("Run.Battle.Hero.ActivatedSubFormations"));
        MechListView.SetAddress(new Address("Run.Battle.MechBag"));

        ConfigureInteractDelegate();

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    public override void Refresh()
    {
        FieldView.Refresh();
        HandView.Refresh();
        FormationListView.Refresh();
        MechListView.Refresh();
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(5,
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
                if (view is FormationIconView)
                    return 4;
                return null;
            },
            dragDropTable: new Action<IInteractable, IInteractable>[]
            {
                /*                      RunSkill,   SkillInventory, SkillSlot(Hero), Mech,       FormationIconView*/
                /* RunSkill          */ TryMerge,   null,           TryEquipSkill,   null,       null,
                /* SkillInventory    */ null,       null,           null,            null,       null,
                /* SkillSlot(Hero)   */ TryUnequip, TryUnequip,     TrySwap,         TryUnequip, null,
                /* Mech              */ null,       null,           TryEquipMech,    null,       null,
                /* FormationIconView */ null,       null,           null,            null,       null,
            });

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((HandSkillView)v).HoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((HandSkillView)v).UnhoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((HandSkillView)v).PointerMove(d));
        InteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 0, (v, d) => ((HandSkillView)v).BeginDrag(d));
        InteractDelegate.SetHandle(InteractDelegate.END_DRAG, 0, (v, d) => ((HandSkillView)v).EndDrag(d));
        InteractDelegate.SetHandle(InteractDelegate.DRAG, 0, (v, d) => ((HandSkillView)v).Drag(d));

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 2, (v, d) => ((FieldSlotView)v).HoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 2, (v, d) => ((FieldSlotView)v).UnhoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 2, (v, d) => ((FieldSlotView)v).PointerMove(d));
        InteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 2, (v, d) => ((FieldSlotView)v).BeginDrag(d));
        InteractDelegate.SetHandle(InteractDelegate.END_DRAG, 2, (v, d) => ((FieldSlotView)v).EndDrag(d));
        InteractDelegate.SetHandle(InteractDelegate.DRAG, 2, (v, d) => ((FieldSlotView)v).Drag(d));

        InteractDelegate.SetHandle(InteractDelegate.BEGIN_DRAG, 3, (v, d) => ((MechView)v).BeginDrag(d));
        InteractDelegate.SetHandle(InteractDelegate.END_DRAG, 3, (v, d) => ((MechView)v).EndDrag(d));
        InteractDelegate.SetHandle(InteractDelegate.DRAG, 3, (v, d) => ((MechView)v).Drag(d));

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 4, (v, d) => ((FormationIconView)v).PointerEnter(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 4, (v, d) => ((FormationIconView)v).PointerExit(v, d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 4, (v, d) => ((FormationIconView)v).PointerMove(v, d));

        FieldView.SetDelegate(InteractDelegate);
        HandView.SetDelegate(InteractDelegate);
        FormationListView.SetDelegate(InteractDelegate);
        MechListView.SetDelegate(InteractDelegate);
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
        FieldView.Refresh();
        RunCanvas.Instance.NodeLayer.Refresh();
    }

    private void TryEquipMech(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        Mech toEquip = from.Get<Mech>();
        SkillSlot slot = to.Get<SkillSlot>();
        env.TryEquipMech(toEquip, slot);
        from.Refresh();
        FieldView.Refresh();
        RunCanvas.Instance.NodeLayer.Refresh();
    }

    private void TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        SkillSlot slot = from.Get<SkillSlot>();
        env.TryUnequip(slot, null);
        FieldView.Refresh();
        RunCanvas.Instance.NodeLayer.Refresh();
    }

    private void TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Battle").Get<RunEnvironment>();
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        env.TrySwap(fromSlot, toSlot);
        FieldView.Refresh();
        RunCanvas.Instance.NodeLayer.Refresh();
    }

    #endregion

    private void Sort()
    {
        HandView.Get<SkillInventory>().SortByComparisonId(0);
        RunCanvas.Instance.Refresh();
    }

    public void SetLocked(bool locked)
    {
        if (locked)
        {
            ToggleButton.interactable = false;
            ToggleImage.color = new Color(0.6f, 0.6f, 0.6f, 0.43f);
        }
        else
        {
            ToggleButton.interactable = true;
            ToggleImage.color = Color.white;
        }
    }
}
