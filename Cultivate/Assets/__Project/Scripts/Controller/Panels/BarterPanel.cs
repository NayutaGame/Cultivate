
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public XView LeftBucketDropZone;
    public RectTransform LeftBucketOffset;
    public ListView LeftBucket;
    public HorizontalLayoutGroup LeftLayout;
    
    public XView RightBucketDropZone;
    public RectTransform RightBucketOffset;
    public ListView RightBucket;
    public HorizontalLayoutGroup RightLayout;
    
    public ListView Board;

    public TMP_Text RefreshCostText;
    public CLButtonPatternA RefreshItemsButton;
    public CLButtonPatternA ExitButton;

    public CLButtonPatternA ExchangeButton;

    private Address _address;

    private Tween _weightHandle;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        LeftBucketDropZone.CheckAwake();
        RightBucketDropZone.CheckAwake();

        _address = new Address("Run.Environment.ActivePanel");
        LeftBucket.SetAddress(_address.Append(".LeftBucketItems"));
        LeftBucket.NeuronBundle.DropNeuron.Join(LeftBucketDrop);
        LeftBucket.ItemCountChanged.Join(LeftBucketRefreshLayoutSpacing);
        LeftBucketDropZone.GetInteractBehaviour().NeuronBundle.DropNeuron.Join(LeftBucketDrop);
        
        RightBucket.SetAddress(_address.Append(".RightBucketItems"));
        RightBucket.NeuronBundle.DropNeuron.Join(RightBucketDrop);
        RightBucket.ItemCountChanged.Join(RightBucketRefreshLayoutSpacing);
        RightBucketDropZone.GetInteractBehaviour().NeuronBundle.DropNeuron.Join(RightBucketDrop);
        
        Board.SetAddress(_address.Append(".BoardItems"));
        Board.NeuronBundle.DropNeuron.Join(BoardDrop);
        
        ExchangeButton.LeftClickNeuron.Join(Exchange);
    }

    private void OnEnable()
    {
        RefreshItemsButton.LeftClickNeuron.Add(RefreshItems);
        ExitButton.LeftClickNeuron.Add(ExitShop);
        RunManager.Instance.Environment.FromHandToBarterNeuron.Add(FromHandToBarterStaging);
        RunManager.Instance.Environment.FromFieldToBarterNeuron.Add(FromFieldToBarterStaging);
        RunManager.Instance.Environment.FromBarterToHandNeuron.Add(FromBarterToHandStaging);
        RunManager.Instance.Environment.FromBarterToFieldNeuron.Add(FromBarterToFieldStaging);
        RunManager.Instance.Environment.FromBoardToRightBucketNeuron.Add(FromBoardToRightBucketStaging);
        RunManager.Instance.Environment.FromRightBucketToBoardNeuron.Add(FromRightBucketToBoardStaging);
        RunManager.Instance.Environment.BarterClearRightBucketItemsNeuron.Add(ClearRightBucketStaging);
        RunManager.Instance.Environment.ExchangeSkillNeuron.Add(ExchangeStaging);
        RunManager.Instance.Environment.BarterWeightIsUpdatedNeuron.Add(WeightIsChanged);
        SmallRefresh();
    }

    private void OnDisable()
    {
        RefreshItemsButton.LeftClickNeuron.Remove(RefreshItems);
        ExitButton.LeftClickNeuron.Remove(ExitShop);
        RunManager.Instance.Environment.FromHandToBarterNeuron.Remove(FromHandToBarterStaging);
        RunManager.Instance.Environment.FromFieldToBarterNeuron.Remove(FromFieldToBarterStaging);
        RunManager.Instance.Environment.FromBarterToHandNeuron.Remove(FromBarterToHandStaging);
        RunManager.Instance.Environment.FromBarterToFieldNeuron.Remove(FromBarterToFieldStaging);
        RunManager.Instance.Environment.FromBoardToRightBucketNeuron.Remove(FromBoardToRightBucketStaging);
        RunManager.Instance.Environment.FromRightBucketToBoardNeuron.Remove(FromRightBucketToBoardStaging);
        RunManager.Instance.Environment.BarterClearRightBucketItemsNeuron.Remove(ClearRightBucketStaging);
        RunManager.Instance.Environment.ExchangeSkillNeuron.Remove(ExchangeStaging);
        RunManager.Instance.Environment.BarterWeightIsUpdatedNeuron.Remove(WeightIsChanged);
    }

    public override void Refresh()
    {
        LeftBucket.Sync();
        RightBucket.Sync();
        Board.Sync();
        SmallRefresh();
    }

    private void LeftBucketRefreshLayoutSpacing()
    {
        float L = LeftBucket.GetRect().rect.width;
        float l = 160;
        int n = LeftBucket.GetCount();
        float s;
        if (n < L / l)
            s = 0;
        else
            s = -(n * l - L) / (n - 1);
        LeftLayout.spacing = s;
    }

    private void RightBucketRefreshLayoutSpacing()
    {
        float L = RightBucket.GetRect().rect.width;
        float l = 160;
        int n = RightBucket.GetCount();
        float s;
        if (n < L / l)
            s = 0;
        else
            s = -(n * l - L) / (n - 1);
        RightLayout.spacing = s;
    }

    private void WeightIsChanged(int weight)
    {
        SmallRefresh();
    }

    private void SmallRefresh()
    {
        RefreshWeight();
        RefreshExchangeButton();
        RefreshRefreshItemsButton();
    }

    private void RefreshWeight()
    {
        BarterCell barterCell = _address.Get<ICellAdapter>().AsCell() as BarterCell;
        
        float y = MapWeightToY(barterCell.Weight);

        _weightHandle?.Kill();
        
        _weightHandle = DOTween.Sequence()
            .Append(LeftBucketOffset.DOAnchorPosY(y, 2f).SetEase(Ease.OutElastic))
            .Join(RightBucketOffset.DOAnchorPosY(-y, 2f).SetEase(Ease.OutElastic));

        _weightHandle.SetAutoKill(true);
        _weightHandle.Restart();
    }

    private float MapWeightToY(int weight)
    {
        const float WEIGHT_SCALE = 5f;
        const float MAX_Y = 33f;
        
        float normalizedWeight = (float)Math.Tanh(weight / WEIGHT_SCALE);
        return normalizedWeight * MAX_Y;
    }

    private void RefreshExchangeButton()
    {
        BarterCell barterCell = _address.Get<ICellAdapter>().AsCell() as BarterCell;
        ExchangeButton.SetStateToActiveIf(barterCell.CanExchange());
    }

    private void RefreshRefreshItemsButton()
    {
        BarterCell barterCell = _address.Get<ICellAdapter>().AsCell() as BarterCell;
        if (!barterCell.RefreshItemsIsAllowed())
        {
            RefreshItemsButton.gameObject.SetActive(false);
            return;
        }
        
        RefreshItemsButton.gameObject.SetActive(true);
        RefreshCostText.text = barterCell.GetRefreshItemsDescription();
        RefreshItemsButton.SetStateToActiveIf(barterCell.RefreshItemsIsAffordable());
    }

    private void RefreshItems(InteractBehaviour ib, PointerEventData d)
    {
        BarterCell barterCell = _address.Get<ICellAdapter>().AsCell() as BarterCell;
        barterCell.RefreshItemsProcedure();
        
        LeftBucket.Sync();
        SmallRefresh();
    }

    private void Exchange(InteractBehaviour ib, PointerEventData d)
    {
        BarterCell cell = _address.Get<ICellAdapter>().AsCell() as BarterCell;
        cell.ExchangeProcedure();
    }

    private void ExitShop(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ExitShopProcedure();
    }

    private void LeftBucketDrop(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        IDeckIndex fromIndex = CanvasManager.Instance.RunCanvas.GetDeckIndex(from);
        if (fromIndex == null)
            return;
        
        RunManager.Instance.Environment.MoveSkillProcedure(fromIndex, new NextBarterDeckIndexDefinition());
    }

    private void RightBucketDrop(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        // from is from board
        // to is from right
        BarterCell cell = _address.Get<ICellAdapter>().AsCell() as BarterCell;

        BarterBoardSlot fromSlot = from.Get<BarterBoardSlot>();
        if (fromSlot == null)
            return;

        if (!cell.BoardItems.Contains(fromSlot))
            return;
        
        int fromIndex = cell.BoardItems.IndexOf(fromSlot);
        cell.FromBoardToRightBucketProcedure(fromIndex);
    }

    private void BoardDrop(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        // from is from right bucket
        // to is from board
        BarterCell cell = _address.Get<ICellAdapter>().AsCell() as BarterCell;

        SkillGhost fromSkill = from.Get<SkillGhost>();
        if (fromSkill == null || !cell.RightBucketItems.Contains(fromSkill))
            return;

        BarterBoardSlot toSlot = to.Get<BarterBoardSlot>();
        if (toSlot == null || !cell.BoardItems.Contains(toSlot))
            return;
        
        int fromIndex = cell.RightBucketItems.IndexOf(fromSkill);
        int toIndex = cell.BoardItems.IndexOf(toSlot);
        cell.FromRightBucketToBoardProcedure(fromIndex, toIndex);
    }

    private void ClearRightBucketStaging()
    {
        int rightBucketItemCount = RightBucket.GetCount();
        for (int i = 0; i < rightBucketItemCount; i++)
        {
            RightBucket.RemoveItemAt(0);
        }
    }

    private void FromBoardToRightBucketStaging(FromBoardToRightBucketDetails d)
    {
        RightBucket.AddItem();

        SlotView from = Board.ViewFromIndex(d.FromIndex);
        SlotView to = RightBucket.LastView();
        
        to.Refresh();
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        Board.Modified(d.FromIndex);
        
        AudioManager.Play("CardPlacement");
    }

    private void FromRightBucketToBoardStaging(FromRightBucketToBoardDetails d)
    {
        SlotView from = RightBucket.ViewFromIndex(d.FromIndex);
        SlotView to = Board.ViewFromIndex(d.ToIndex);
        to.Refresh();
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        RightBucket.RemoveItemAt(d.FromIndex);
        Board.Modified(d.ToIndex);
        
        AudioManager.Play("CardPlacement");
    }

    private void ExchangeStaging(ExchangeSkillDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        
        int leftBucketItemCount = LeftBucket.GetCount();
        for (int i = 0; i < leftBucketItemCount; i++)
            LeftBucket.RemoveItemAt(0);

        // Tween seq = DOTween.Sequence();
        
        int rightBucketItemCount = RightBucket.GetCount();
        for (int i = 0; i < rightBucketItemCount; i++)
        {
            deckPanel.HandView.AddItem();
            
            SlotView from = RightBucket.ViewFromIndex(0);
            SlotView to = deckPanel.HandView.LastView();
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
            RightBucket.RemoveItemAt(0);
        }
        
        AudioManager.Play("CardPlacement");
        
        // CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueAnimation(seq);
    }

    #region MoveSkillRelated

    private void FromHandToBarterStaging(FromHandToBarterDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        
        LeftBucket.AddItem();
        
        SlotView from = deckPanel.SkillItemFromDeckIndex(d.FromIndex);
        SlotView to = LeftBucket.LastView();
        
        to.Refresh();
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        from.GetAnimator().SetStateAsync(1);
        
        deckPanel.HandView.RemoveItemAt(d.FromIndex.Index);
        
        AudioManager.Play("CardPlacement");
    }

    private void FromFieldToBarterStaging(FromFieldToBarterDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        
        LeftBucket.AddItem();
        
        SlotView from = deckPanel.SkillItemFromDeckIndex(d.FromIndex);
        SlotView to = LeftBucket.LastView();
        
        to.Refresh();
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        deckPanel.PlayerEntity.FieldView.Modified(d.FromIndex.Index);
        
        AudioManager.Play("CardPlacement");
    }

    private void FromBarterToHandStaging(FromBarterToHandDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        deckPanel.HandView.AddItem();
        
        SlotView from = LeftBucket.ViewFromIndex(d.FromIndex.Index);
        SlotView to = deckPanel.LatestSkillItem();
        
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        from.GetAnimator().SetStateAsync(1);
        
        LeftBucket.RemoveItemAt(d.FromIndex.Index);
        
        AudioManager.Play("CardPlacement");
    }

    private void FromBarterToFieldStaging(FromBarterToFieldDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        
        SlotView from = LeftBucket.ViewFromIndex(d.FromIndex.Index);
        SlotView to = deckPanel.SkillItemFromDeckIndex(d.ToIndex);
        to.Refresh();
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        LeftBucket.RemoveItemAt(d.FromIndex.Index);
        deckPanel.PlayerEntity.FieldView.Modified(d.ToIndex.Index);
        
        AudioManager.Play("CardPlacement");
    }

    #endregion
}
