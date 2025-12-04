
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarterPanel : Panel
{
    public XView LeftBucketDropZone;
    public XView RightBucketDropZone;

    public RectTransform LeftBucketRect;
    public RectTransform RightBucketRect;
    
    public ListView LeftBucket;
    public ListView RightBucket;
    public ListView Board;

    public TMP_Text RefreshCostText;
    public Button4State RefreshItemsButton;
    public Button4State ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        LeftBucketDropZone.CheckAwake();
        RightBucketDropZone.CheckAwake();

        _address = new Address("Run.Environment.ActivePanel");
        LeftBucket.SetAddress(_address.Append(".LeftBucketItems"));
        LeftBucket.DropNeuron.Join(LeftBucketDrop);
        LeftBucketDropZone.GetInteractBehaviour().DropNeuron.Join(LeftBucketDrop);
    }

    private void OnEnable()
    {
        RefreshItemsButton.LeftClickNeuron.Add(RefreshItems);
        ExitButton.LeftClickNeuron.Add(ExitShop);
        RunManager.Instance.Environment.FromHandToBarterNeuron.Add(FromHandToBarterStaging);
        RunManager.Instance.Environment.FromFieldToBarterNeuron.Add(FromFieldToBarterStaging);
        RunManager.Instance.Environment.FromBarterToHandNeuron.Add(FromBarterToHandStaging);
        RunManager.Instance.Environment.FromBarterToFieldNeuron.Add(FromBarterToFieldStaging);
        RefreshRefreshItemsButton();
    }

    private void OnDisable()
    {
        RefreshItemsButton.LeftClickNeuron.Remove(RefreshItems);
        ExitButton.LeftClickNeuron.Remove(ExitShop);
        RunManager.Instance.Environment.FromHandToBarterNeuron.Remove(FromHandToBarterStaging);
        RunManager.Instance.Environment.FromFieldToBarterNeuron.Remove(FromFieldToBarterStaging);
        RunManager.Instance.Environment.FromBarterToHandNeuron.Remove(FromBarterToHandStaging);
        RunManager.Instance.Environment.FromBarterToFieldNeuron.Remove(FromBarterToFieldStaging);
    }

    public override void Refresh()
    {
        LeftBucket.Refresh();
        RefreshRefreshItemsButton();
    }

    private void RefreshRefreshItemsButton()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        BarterCell barterCell = cellAdapter.AsCell() as BarterCell;
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
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        BarterCell barterCell = cellAdapter.AsCell() as BarterCell;
        barterCell.RefreshItemsProcedure();
        
        LeftBucket.Sync();
        RefreshRefreshItemsButton();
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
