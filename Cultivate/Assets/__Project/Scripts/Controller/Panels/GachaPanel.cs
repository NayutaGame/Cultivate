
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaPanel : Panel
{
    public TMP_Text PriceTag;
    public Button4State BuyButton;
    public Button4State ExitButton;
    public ListView ListView;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Items"));
    }

    private void OnEnable()
    {
        BuyButton.LeftClickNeuron.Add(Gacha);
        ExitButton.LeftClickNeuron.Add(ExitShop);
        RunManager.Instance.Environment.GachaNeuron.Add(CanvasManager.Instance.RunCanvas.GachaStaging);
    }

    private void OnDisable()
    {
        BuyButton.LeftClickNeuron.Remove(Gacha);
        ExitButton.LeftClickNeuron.Remove(ExitShop);
        RunManager.Instance.Environment.GachaNeuron.Remove(CanvasManager.Instance.RunCanvas.GachaStaging);
    }

    public override void Refresh()
    {
        ListView.Sync();
        
        GachaCell d = _address.Get<GachaCell>();
        PriceTag.text = $"每抽 {d.GetPrice()} 金";
        BuyButton.SetStateToInactiveFrom(d.ItemsIsEmpty);
    }

    private void Gacha(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.CloseAnnotation();
        
        GachaCell gachaCell = _address.Get<GachaCell>();
        gachaCell.GachaProcedure();
        BuyButton.SetStateToInactiveFrom(gachaCell.ItemsIsEmpty);
    }

    private void ExitShop(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ExitShopProcedure();
    }

    public XView GachaItemFromIndex(int gachaIndex)
    {
        return ListView.ViewFromIndex(gachaIndex);
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0));
}
