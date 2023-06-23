
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
    public SlotInventoryView PlayerHand;
    public SkillInventoryView PlayerInventory;

    public RectTransform _backgroundTransform;
    public RectTransform _spriteTransform;
    public RectTransform _handTransform;

    public override Tween GetShowTween()
        => DOTween.Sequence()
            .Join(_spriteTransform.DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutQuad))
            .Join(_handTransform.DOAnchorPosY(107f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.05f))
            .Join(_backgroundTransform.DOAnchorPosY(-56.5f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.1f));

    public override Tween GetHideTween()
        => DOTween.Sequence()
            .Join(_backgroundTransform.DOAnchorPosY(-434f, 0.3f).SetEase(Ease.InQuad))
            .Join(_handTransform.DOAnchorPosY(-392f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.05f))
            .Join(_spriteTransform.DOAnchorPosY(-600f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.1f));

    public override void Configure()
    {
        PlayerHand.Configure(new IndexPath("Battle.Hero.Slots"));
        PlayerInventory.Configure(new IndexPath("Battle.SkillInventory"));

        DeckToggle.onClick.RemoveAllListeners();
        DeckToggle.onClick.AddListener(ToggleDeck);

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    public override void Refresh()
    {
        PlayerHand.Refresh();
        PlayerInventory.Refresh();
    }

    public void SetInteractDelegate(InteractDelegate interactDelegate)
    {
        PlayerHand.SetDelegate(interactDelegate);
        PlayerInventory.SetDelegate(interactDelegate);
    }

    private void Sort()
    {
        SkillInventory inventory = RunManager.Get<SkillInventory>(new IndexPath("Battle.SkillInventory"));
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
