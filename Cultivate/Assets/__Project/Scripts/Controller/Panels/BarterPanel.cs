
using CLLibrary;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public AnimatedListView BarterItemListView;

    public Button ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        BarterItemListView.SetAddress(_address.Append(".Inventory"));

        BarterItemListView.Traversal().Do(v =>
        {
            BarterItemView barterItemView = (v as DelegatingView).GetDelegatedView() as BarterItemView;
            barterItemView.ExchangeSkillEvent.Join(ExchangeSkill);
        });

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(RunManager.Instance.Environment.ExitShopProcedure);
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.ExchangeSkillNeuron.Add(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.ExchangeSkillNeuron.Remove(CanvasManager.Instance.RunCanvas.ExchangeSkillStaging);
    }

    public override void Refresh()
    {
        BarterItemListView.Refresh();
    }

    public XView BarterItemFromIndex(int commodityIndex)
    {
        return BarterItemListView.ViewFromIndex(commodityIndex);
    }

    private void ExchangeSkill(BarterItem barterItem)
    {
        BarterPanelDescriptor barterPanelDescriptor = _address.Get<BarterPanelDescriptor>();
        ExchangeSkillDetails details = new(barterItem);
        barterPanelDescriptor.ExchangeSkillProcedure(details);
        // AudioManager.Instance.Play("钱币");
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");
}
