
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
        BuyButton.onClick.AddListener(Gacha);
        
        BuyButton.interactable = !d.ItemsIsEmpty;

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerEnter(ib, d, ib.GetAddress().Append(".Skill"));

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerExit(ib, d, ib.GetAddress().Append(".Skill"));

    private void PointerMove(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerMove(ib, d, ib.GetAddress().Append(".Skill"));

    public override void Refresh()
    {
        base.Refresh();
        ListView.Refresh();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.GachaNeuron.Add(GachaStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GachaNeuron.Remove(GachaStaging);
    }

    private void Gacha()
    {
        GachaPanelDescriptor d = _address.Get<GachaPanelDescriptor>();
        d.Buy();
        BuyButton.interactable = !d.ItemsIsEmpty;
    }

    private void GachaStaging(GachaDetails d)
    {
        InteractBehaviour gachaIB = ListView.InactivePools[0][^1].GetInteractBehaviour();
        InteractBehaviour cardIB = CanvasManager.Instance.RunCanvas.SkillInteractBehaviourFromDeckIndex(d.DeckIndex);

        CanvasManager.Instance.RunCanvas.GachaStaging(cardIB, gachaIB);
        CanvasManager.Instance.SkillAnnotation.PointerExit();
        
        // ExtraBehaviourPivot extraBehaviourPivot = commodityIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        // if (extraBehaviourPivot != null)
        //     extraBehaviourPivot.Disappear();
    }

    private void Exit()
    {
        Signal signal = new ExitShopSignal();
        CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
