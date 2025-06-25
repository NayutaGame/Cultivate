
using DG.Tweening;
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
        
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(Gacha);

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(ExitShop);
    }

    public override void Refresh()
    {
        ListView.Sync();
        
        GachaCell d = _address.Get<GachaCell>();
        PriceTag.text = $"每抽 {d.GetPrice()} 金";
        BuyButton.interactable = !d.ItemsIsEmpty;
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
        
        GachaCell gachaCell = _address.Get<GachaCell>();
        gachaCell.GachaProcedure();
        BuyButton.interactable = !gachaCell.ItemsIsEmpty;
    }

    private void ExitShop()
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
