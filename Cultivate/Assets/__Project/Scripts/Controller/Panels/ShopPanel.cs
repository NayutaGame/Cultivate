
using UnityEngine.EventSystems;

public class ShopPanel : Panel
{
    public FixedListView ListView;
    public CLButtonPatternA ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Commodities"));
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.BuySkillNeuron.Add(CanvasManager.Instance.RunCanvas.BuySkillStaging);
        RunManager.Instance.Environment.GainGoldNeuron.Add(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(LoseGold);
        ExitButton.LeftClickNeuron.Add(ExitShop);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.BuySkillNeuron.Remove(CanvasManager.Instance.RunCanvas.BuySkillStaging);
        RunManager.Instance.Environment.GainGoldNeuron.Remove(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Remove(LoseGold);
        ExitButton.LeftClickNeuron.Remove(ExitShop);
    }

    public override void Refresh()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        ShopCell pd = cellAdapter.AsCell() as ShopCell;

        ListView.Sync();
    }

    private void GainGold(int value)
        => ListView.Refresh();

    private void LoseGold(int value)
        => ListView.Refresh();

    private void ExitShop(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ExitShopProcedure();
    }

    public XView CommodityItemFromIndex(int commodityIndex)
    {
        return ListView.ViewFromIndex(commodityIndex);
    }
}
