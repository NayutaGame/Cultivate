
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckPanel : Panel
{
    public PropagatePointerEnter DeckOpenZone;
    public PropagatePointerEnter DeckCloseZone;

    public Button SortButton;

    public ListView FieldView;
    public ListView HandView;
    public ListView FormationListView;
    public ListView MechListView;

    public RectTransform _fieldTransform;
    public RectTransform _handTransform;
    public RectTransform _formationListTransform;
    public RectTransform _mechListTransform;

    public override void Configure()
    {
        base.Configure();

        DeckOpenZone._onPointerEnter = TryShow;
        DeckCloseZone._onPointerEnter = TryHide;
        SetLocked(false);

        FieldView.SetAddress(new Address("Run.Environment.Hero.Slots"));
        // HandView.SetAddress(new Address("Run.Environment.SkillInventory"));
        FormationListView.SetAddress(new Address("Run.Environment.Hero.ActivatedSubFormations"));
        MechListView.SetAddress(new Address("Run.Environment.MechBag"));

        ConfigureInteractDelegate();

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    public override void Refresh()
    {
        base.Refresh();
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
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill lhs = from.Get<RunSkill>();
        RunSkill rhs = to.Get<RunSkill>();
        bool success = env.TryMerge(lhs, rhs);
        if (!success)
            return;

        AudioManager.Play("CardUpgrade");
    }

    private void TryEquipSkill(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        RunSkill toEquip = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();
        bool success = env.TryEquipSkill(toEquip, slot);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryEquipMech(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        Mech toEquip = from.Get<Mech>();
        SkillSlot slot = to.Get<SkillSlot>();
        bool success = env.TryEquipMech(toEquip, slot);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        from.Refresh();
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot slot = from.Get<SkillSlot>();
        bool success = env.TryUnequip(slot, null);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    private void TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment env = new Address("Run.Environment").Get<RunEnvironment>();
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        bool success = env.TrySwap(fromSlot, toSlot);
        if (!success)
            return;

        AudioManager.Play("CardPlacement");
        FieldView.Refresh();
        CanvasManager.Instance.RunCanvas.NodeLayer.Refresh();
    }

    #endregion

    private void Sort()
    {
        HandView.Get<SkillInventory>().SortByComparisonId(0);
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void TryShow(PointerEventData eventData)
    {
        SetShowing(true);
    }

    private void TryHide(PointerEventData eventData)
    {
        SetShowing(false);
    }

    private bool _locked;
    public void SetLocked(bool locked)
    {
        if (_locked == locked)
            return;
        _locked = locked;
        DeckCloseZone.gameObject.SetActive(!_locked);
    }

    public override Tween ShowAnimation()
        => DOTween.Sequence()
            .AppendCallback(HandView.BigRefresh)
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(false))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(!_locked))
            .Join(_handTransform.                  DOAnchorPosY(-69.5f+445-672, 0.3f).SetEase(Ease.OutQuad).SetDelay(0f))
            .Join(_fieldTransform.                 DOAnchorPosY(94f+403-630, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.06f))
            .Join(_formationListTransform. DOAnchorPosY(200f+364-591, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.09f))
            .Join(_mechListTransform.               DOAnchorPosY(-255f+375-602, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.12f));

    public override Tween HideAnimation()
        => DOTween.Sequence()
            .AppendCallback(() => DeckOpenZone.gameObject.SetActive(true))
            .AppendCallback(() => DeckCloseZone.gameObject.SetActive(false))
            .Join(_handTransform.                  DOAnchorPosY(-672, 0.3f).SetEase(Ease.InQuad).SetDelay(0f))
            .Join(_fieldTransform.                 DOAnchorPosY(-630, 0.3f).SetEase(Ease.InQuad).SetDelay(0.06f))
            .Join(_formationListTransform. DOAnchorPosY(-591, 0.3f).SetEase(Ease.InQuad).SetDelay(0.09f))
            .Join(_mechListTransform.               DOAnchorPosY(-602, 0.3f).SetEase(Ease.InQuad).SetDelay(0.12f));
}
