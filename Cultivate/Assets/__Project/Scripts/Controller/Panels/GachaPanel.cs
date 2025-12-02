
using DG.Tweening;
using TMPro;
using UnityEngine;
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
    
    public static readonly int PICKING = 1;

    protected override Animator InitAnimator()
    {
        Animator animator = new(3, "GachaPanel");
        animator[HIDE, IDLE] = EnterIdle;
        animator[IDLE, PICKING] = Idle2Picking;
        animator[PICKING, IDLE] = Picking2Idle;
        animator[ANY, HIDE] = EnterHide;
        return animator;
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
        
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        GachaCell cell = cellAdapter.AsCell() as GachaCell;
        PriceTag.text = $"每抽 {cell.GetPrice()} 金";
        BuyButton.SetStateToInactiveFrom(cell.ItemsIsEmpty);
    }

    private void Gacha(InteractBehaviour ib, PointerEventData d)
    {
        // CanvasManager.Instance.CloseAnnotation();
        //
        // ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        // GachaCell cell = cellAdapter.AsCell() as GachaCell;
        // cell.GachaProcedure();
        // BuyButton.SetStateToInactiveFrom(cell.ItemsIsEmpty && !cell.IsAffordable());
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
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE));
    
    // 两个button锁住无法交互
    // ListView所有item变Free
    // ListView所有item翻到背面
    // ListView 洗牌
    // Model中随机化牌序
    // ListView 排列
    public Tween Idle2Picking()
    {
        HorizontalLayoutGroup hLayout;
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() => BuyButton.SetStateToInactiveFrom(true))
            .AppendCallback(() => ExitButton.SetStateToInactiveFrom(true))
            .AppendCallback(() =>
            {
                foreach (SlotView view in ListView.TraversalActive())
                    view.GetAnimator().SetState(SlotView.FREE);
            })
            .AppendCallback(() =>
            {
                foreach (SlotView view in ListView.TraversalActive())
                    (view.GetContentView() as GachaItemView).GetBehaviour<FlipBehaviour>().SetFlipped(true);
            })
            // .AppendInterval(0.3f)
            // .AppendCallback(() =>
            // {
            //     // hLayout.
            // })
            // .AppendCallback(() =>
            // {
            //     ShuffleModel();
            // })
            .AppendCallback(() =>
            {
                ListView.Sync();
                ListView.RefreshPivotsAsync();
            });
        
        return seq;
    }

    // 被抽到的牌获得Staging
    // ListView 排列
    // ListView 翻回正面
    // 两个Button允许交互
    public Tween Picking2Idle()
    {
        Sequence seq = DOTween.Sequence();
        
        // 1. 被抽到的牌获得Staging (这个在 GachaStaging 中已经处理)
        // 这里只需要等待 Staging 完成
        seq.AppendInterval(0.2f)
            .AppendCallback(() => // 3. ListView 翻回正面
            {
                foreach (SlotView view in ListView.TraversalActive())
                    (view.GetContentView() as GachaItemView).GetBehaviour<FlipBehaviour>().SetFlipped(false);
            })
            .AppendCallback(() =>
            {
                ListView.RefreshPivotsAsync();
            })
            .AppendInterval(0.3f)
            .AppendCallback(() => // 4. 两个Button允许交互
            {
                ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
                GachaCell cell = cellAdapter.AsCell() as GachaCell;
                BuyButton.SetStateToInactiveFrom(cell.ItemsIsEmpty || cell.IsAffordable());
                ExitButton.SetStateToInactiveFrom(false);
            });
        
        return seq;
    }

    public override Tween EnterHide()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));
}
