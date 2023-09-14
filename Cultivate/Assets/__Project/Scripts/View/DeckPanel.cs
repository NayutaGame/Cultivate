
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeckPanel : Panel
{
    public Image OriginalImage;
    public Button DeckToggle;

    public void ClearToggleDeckEvent() => ToggleDeckEvent = null;
    public event Action ToggleDeckEvent;
    private void ToggleDeck() => ToggleDeckEvent?.Invoke();

    public Image PlayerSprite;
    public Button SortButton;
    public ListView FieldView; // SlotView
    public SkillInventoryView HandView;
    public ListView PlayerSubFormationInventory; // FormationView
    public ListView MechBagView; // MechView

    public RectTransform _deckTransform;
    public RectTransform _spriteTransform;
    public RectTransform _handTransform;
    public RectTransform _subFormationInventoryTransform;
    public RectTransform _mechBagTransform;

    public override Tween GetShowTween()
        => DOTween.Sequence()
            .Join(_deckTransform.                  DOAnchorPosY(-69.5f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0f))
            .Join(_spriteTransform.                DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.03f))
            .Join(_handTransform.                  DOAnchorPosY(94f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.06f))
            .Join(_subFormationInventoryTransform. DOAnchorPosY(200f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.09f))
            .Join(_mechBagTransform.               DOAnchorPosY(-255f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.12f));

    public override Tween GetHideTween()
        => DOTween.Sequence()
            .Join(_deckTransform.                  DOAnchorPosY(-445f, 0.3f).SetEase(Ease.InQuad).SetDelay(0f))
            .Join(_spriteTransform.                DOAnchorPosY(-626f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.03f))
            .Join(_handTransform.                  DOAnchorPosY(-403f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.06f))
            .Join(_subFormationInventoryTransform. DOAnchorPosY(-364f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.09f))
            .Join(_mechBagTransform.               DOAnchorPosY(-375, 0.3f).SetEase(Ease.InQuad).SetDelay(0.12f));

    public override void Configure()
    {
        FieldView.Configure(new Address("Run.Battle.Hero.Slots"));
        HandView.Configure(new Address("Run.Battle.SkillInventory"));
        PlayerSubFormationInventory.Configure(new Address("Run.Battle.Hero.ActivatedSubFormations"));
        MechBagView.Configure(new Address("Run.Battle.MechBag.List"));

        DeckToggle.onClick.RemoveAllListeners();
        DeckToggle.onClick.AddListener(ToggleDeck);

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    public override void Refresh()
    {
        FieldView.Refresh();
        HandView.Refresh();
        PlayerSubFormationInventory.Refresh();
        MechBagView.Refresh();
    }

    public void SetInteractDelegate(InteractDelegate interactDelegate)
    {
        FieldView.SetDelegate(interactDelegate);
        HandView.SetDelegate(interactDelegate);
        MechBagView.SetDelegate(interactDelegate);
    }

    private void Sort()
    {
        SkillInventory inventory = new Address("Run.Battle.SkillInventory").Get<SkillInventory>();
        inventory.SortByComparisonId(0);
        RunCanvas.Instance.Refresh();
    }

    public void SetLocked(bool locked)
    {
        if (locked)
        {
            DeckToggle.interactable = false;
            OriginalImage.color = new Color(0.6f, 0.6f, 0.6f, 0.43f);
        }
        else
        {
            DeckToggle.interactable = true;
            OriginalImage.color = Color.white;
        }
    }
}
