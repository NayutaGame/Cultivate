
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public ListView BarterItemListView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        BarterItemListView.SetAddress(_address.Append(".Inventory"));
        BarterItemListView.PointerEnterNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerEnter, PlayCardHoverSFX);
        BarterItemListView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit);
        BarterItemListView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);

        foreach (ItemBehaviour itemBehaviour in BarterItemListView.ActivePool)
        {
            BarterItemView barterItemView = itemBehaviour.GetView() as BarterItemView;
            barterItemView.ClearExchangeEvent();
            barterItemView.ExchangeEvent += ExchangeEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        BarterItemListView.Refresh();
    }

    private void ExchangeEvent(BarterItem barterItem)
    {
        BarterPanelDescriptor d = _address.Get<BarterPanelDescriptor>();
        d.Exchange(barterItem);
        // AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        Signal signal = new ExitShopSignal();
        CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");
}
