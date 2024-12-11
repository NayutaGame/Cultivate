
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaPanel : Panel
{
    public TMP_Text PriceTag;
    public Button BuyButton;
    public Button ExitButton;
    public AnimatedListView ListView;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Items"));
        
        GachaPanelDescriptor d = _address.Get<GachaPanelDescriptor>();
        PriceTag.text = $"每抽 {d.GetPrice()} 金";
        
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(Gacha);
        
        BuyButton.interactable = !d.ItemsIsEmpty;

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(RunManager.Instance.Environment.ExitShopProcedure);
    }

    public override void Refresh()
    {
        base.Refresh();
        ListView.Refresh();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.GachaNeuron.Add(CanvasManager.Instance.RunCanvas.GachaStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GachaNeuron.Remove(CanvasManager.Instance.RunCanvas.GachaStaging);
    }

    private void Gacha()
    {
        CanvasManager.Instance.SkillAnnotation.PointerExit();
        
        GachaPanelDescriptor gachaPanelDescriptor = _address.Get<GachaPanelDescriptor>();
        gachaPanelDescriptor.GachaProcedure();
        BuyButton.interactable = !gachaPanelDescriptor.ItemsIsEmpty;
    }

    public XView GachaItemFromIndex(int gachaIndex)
    {
        return ListView.ViewFromIndex(gachaIndex);
    }

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
