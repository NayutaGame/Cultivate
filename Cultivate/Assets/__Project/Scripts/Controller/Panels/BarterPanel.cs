
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public ListView ListView;

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
        ExitButton.LeftClickNeuron.Add(ExitShop);
        RunManager.Instance.Environment.ExchangeSkillNeuron.Add(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
        RunManager.Instance.Environment.MergeNeuron.Add(Refresh);
    }

    private void OnDisable()
    {
        ExitButton.LeftClickNeuron.Remove(ExitShop);
        RunManager.Instance.Environment.ExchangeSkillNeuron.Remove(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
        RunManager.Instance.Environment.MergeNeuron.Remove(Refresh);
    }

    private void Refresh(MergeDetails d)
        => Refresh();

    public override void Refresh()
    {
        ListView.Refresh();
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
