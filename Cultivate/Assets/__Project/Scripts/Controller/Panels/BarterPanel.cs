
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public ListView ListView;

    public Button ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Inventory"));

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(ExitShop);
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.ExchangeSkillNeuron.Add(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
        RunManager.Instance.Environment.MergeNeuron.Add(Refresh);
    }

    private void OnDisable()
    {
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

    private void ExitShop()
    {
        RunManager.Instance.Environment.ExitShopProcedure();
    }
}
