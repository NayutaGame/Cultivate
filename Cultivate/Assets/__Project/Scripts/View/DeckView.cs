using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class DeckView : MonoBehaviour
{
    public Image PlayerSprite;
    public Button ToggleButton;
    public Button SortButton;
    public SlotInventoryView PlayerHand;
    public SkillInventoryView PlayerInventory;

    public RectTransform _backgroundTransform;
    public RectTransform _spriteTransform;
    public RectTransform _handTransform;

    private InteractDelegate InteractDelegate;

    public void Configure()
    {
        _showTween = GetShowTween();
        _hideTween = GetHideTween();

        _showing = false;

        ConfigureInteractDelegate();

        PlayerHand.Configure(new IndexPath("Battle.Hero.Slots"));
        PlayerHand.SetDelegate(InteractDelegate);
        PlayerInventory.Configure(new IndexPath("Battle.SkillInventory"));
        PlayerInventory.SetDelegate(InteractDelegate);

        ToggleButton.onClick.AddListener(Toggle);
        SortButton.onClick.AddListener(Sort);
    }

    public void Refresh()
    {
        PlayerHand.Refresh();
        PlayerInventory.Refresh();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(3,
            getId: view =>
            {
                object item = RunManager.Get<object>(view.GetIndexPath());
                if (item is RunSkill)
                    return 0;
                if (item is SkillInventory)
                    return 1;
                if (item is SkillSlot)
                    return 2;
                return null;
            },
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero) */
                /* RunSkill         */ TryMerge,   null,           TryEquip,
                /* SkillInventory   */ null,       null,           null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,
            });
    }

    private bool TryMerge(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        RunSkill lhs = RunManager.Get<RunSkill>(from.GetIndexPath());
        RunSkill rhs = RunManager.Get<RunSkill>(to.GetIndexPath());
        return runEnvironment.TryMerge(lhs, rhs);
    }

    private bool TryEquip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        RunSkill toEquip = RunManager.Get<RunSkill>(from.GetIndexPath());
        SkillSlot slot = RunManager.Get<SkillSlot>(to.GetIndexPath());
        return runEnvironment.TryEquip(toEquip, slot);
    }

    private bool TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        SkillSlot slot = RunManager.Get<SkillSlot>(from.GetIndexPath());
        return runEnvironment.TryUnequip(slot, null);
    }

    private bool TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(new IndexPath("Battle"));
        SkillSlot fromSlot = RunManager.Get<SkillSlot>(from.GetIndexPath());
        SkillSlot toSlot = RunManager.Get<SkillSlot>(to.GetIndexPath());
        return runEnvironment.TrySwap(fromSlot, toSlot);
    }

    private bool _showing;

    public bool Showing
    {
        get => _showing;
        set
        {
            _showing = value;
            if (_showing)
                _showTween.Restart();
            else
                _hideTween.Restart();
        }
    }

    public void Toggle()
    {
        Showing = !Showing;
    }

    private Tween _showTween;
    private Tween _hideTween;

    private Tween GetShowTween() => DOTween.Sequence().SetAutoKill(false)
        // .AppendCallback(() => Mask.gameObject.SetActive(true))
        // .Append(Mask.DOFade(0.8f, 0.4f).SetEase(Ease.OutQuad))
        .Append(_spriteTransform.DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutQuad))
        .Join(_handTransform.DOAnchorPosY(107f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.05f))
        .Join(_backgroundTransform.DOAnchorPosY(-56.5f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.1f));

    private Tween GetHideTween() => DOTween.Sequence().SetAutoKill(false)
        .Append(_backgroundTransform.DOAnchorPosY(-434f, 0.3f).SetEase(Ease.InQuad))
        .Join(_handTransform.DOAnchorPosY(-392f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.05f))
        .Join(_spriteTransform.DOAnchorPosY(-600f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.1f));
        // .Join(Mask.DOFade(0f, 0.4f).SetEase(Ease.InQuad))
        // .AppendCallback(() => Mask.gameObject.SetActive(false));

    private void Sort()
    {
        SkillInventory inventory = RunManager.Get<SkillInventory>(new IndexPath("Battle.SkillInventory"));
        inventory.SortByComparisonId(0);
        RunCanvas.Instance.Refresh();
    }
}
