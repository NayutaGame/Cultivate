
using TMPro;
using UnityEngine.EventSystems;

public class BarterPanel : Panel
{
    public ListView ListView;

    public TMP_Text RefreshItemsText;
    public Button4State RefreshItemsButton;
    public Button4State ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Inventory"));
    }

    private void OnEnable()
    {
        RefreshItemsButton.LeftClickNeuron.Add(RefreshItems);
        ExitButton.LeftClickNeuron.Add(ExitShop);
        RunManager.Instance.Environment.ExchangeSkillNeuron.Add(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
        RunManager.Instance.Environment.MergeNeuron.Add(Refresh);
        RefreshRefreshItemsButton();
    }

    private void OnDisable()
    {
        RefreshItemsButton.LeftClickNeuron.Remove(RefreshItems);
        ExitButton.LeftClickNeuron.Remove(ExitShop);
        RunManager.Instance.Environment.ExchangeSkillNeuron.Remove(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
        RunManager.Instance.Environment.MergeNeuron.Remove(Refresh);
    }

    private void Refresh(MergeDetails d)
        => Refresh();

    public override void Refresh()
    {
        ListView.Refresh();
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
        RefreshItemsText.text = barterCell.GetRefreshItemsDescription();
        RefreshItemsButton.SetStateToActiveIf(barterCell.RefreshItemsIsAffordable());
    }

    private void RefreshItems(InteractBehaviour ib, PointerEventData d)
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        BarterCell barterCell = cellAdapter.AsCell() as BarterCell;
        barterCell.RefreshItems();
        
        ListView.Sync();
        RefreshRefreshItemsButton();
    }

    public XView BarterItemFromIndex(int commodityIndex)
    {
        return ListView.ViewFromIndex(commodityIndex);
    }

    private void ExitShop(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ExitShopProcedure();
    }
}
