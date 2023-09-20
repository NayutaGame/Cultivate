
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
        // MechListView.SetAddress(new Address("Run.Battle.MechBag.List"));

        InteractDelegate formationIconInteractDelegate = new InteractDelegate(1, v => 0);
        formationIconInteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((FormationView)v).PointerEnter(v, d));
        formationIconInteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((FormationView)v).PointerExit(v, d));
        formationIconInteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((FormationView)v).PointerMove(v, d));
        FormationListView.SetDelegate(formationIconInteractDelegate);

        SortButton.onClick.RemoveAllListeners();
        SortButton.onClick.AddListener(Sort);
    }

    public override void Refresh()
    {
        FieldView.Refresh();
        HandView.Refresh();
        FormationListView.Refresh();
        // MechListView.Refresh();
    }

    public void SetInteractDelegate(InteractDelegate interactDelegate)
    {
        FieldView.SetDelegate(interactDelegate);
        HandView.SetDelegate(interactDelegate);
        // MechListView.SetDelegate(interactDelegate);
    }

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
