
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaPanel : Panel
{
    public TMP_Text PriceTag;
    public Button BuyButton;
    public Button ExitButton;
    public ListView ListView;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Items"));
        ListView.PointerEnterNeuron.Join(PointerEnter);
        ListView.PointerExitNeuron.Join(PointerExit);
        ListView.PointerMoveNeuron.Join(PointerMove);
        
        GachaPanelDescriptor d = _address.Get<GachaPanelDescriptor>();
        PriceTag.text = $"每抽 {d.GetPrice()} 金";
        
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(BuySkill);
        
        BuyButton.interactable = !d.ItemsIsEmpty;

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerEnter(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerExit(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    private void PointerMove(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerMove(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    public override void Refresh()
    {
        base.Refresh();
        ListView.Refresh();
    }

    private void BuySkill()
    {
        GachaPanelDescriptor d = _address.Get<GachaPanelDescriptor>();

        bool success = d.Buy();
        if (!success)
            return;

        BuyStaging();
        CanvasManager.Instance.RunCanvas.Refresh();

        BuyButton.interactable = !d.ItemsIsEmpty;
    }

    private void BuyStaging()
    {
        // AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        Signal signal = new ExitShopSignal();
        CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
