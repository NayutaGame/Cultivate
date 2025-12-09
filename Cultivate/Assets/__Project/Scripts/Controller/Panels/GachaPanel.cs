
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaPanel : Panel
{
    [SerializeField] private TMP_Text PriceTag;
    [SerializeField] private CLButtonPatternA BuyButton;
    [SerializeField] private CLButtonPatternA ExitButton;
    [SerializeField] public ListView ListView;
    [SerializeField] private HorizontalLayoutGroup HLayout;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Items"));
    }

    public static readonly int PICKING = 2;

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
        BuyButton.LeftClickNeuron.Add(GoToPicking);
        ExitButton.LeftClickNeuron.Add(ExitShop);
        RunManager.Instance.Environment.GachaNeuron.Add(CanvasManager.Instance.RunCanvas.GachaStaging);
        RunManager.Instance.Environment.GainGoldNeuron.Add(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(LoseGold);
    }

    private void OnDisable()
    {
        BuyButton.LeftClickNeuron.Remove(GoToPicking);
        ExitButton.LeftClickNeuron.Remove(ExitShop);
        RunManager.Instance.Environment.GachaNeuron.Remove(CanvasManager.Instance.RunCanvas.GachaStaging);
        RunManager.Instance.Environment.GainGoldNeuron.Add(GainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(LoseGold);
    }

    public override void Refresh()
    {
        ListView.Sync();

        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        GachaCell cell = cellAdapter.AsCell() as GachaCell;
        PriceTag.text = $"每抽 {cell.GetPrice()} 金";
        RefreshBuyButton();
    }

    private void GainGold(int value)
        => RefreshBuyButton();

    private void LoseGold(int value)
        => RefreshBuyButton();

    public void RefreshBuyButton()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        GachaCell cell = cellAdapter.AsCell() as GachaCell;
        BuyButton.SetStateToActiveIf(cell.CanCall());
    }

    private void GoToPicking(InteractBehaviour ib, PointerEventData d)
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        GachaCell cell = cellAdapter.AsCell() as GachaCell;
        if (!cell.CanCall())
            return;
        cell.CallProcedure();
        GetAnimator().SetStateAsync(PICKING);
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
            .AppendCallback(RefreshBuyButton)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE));

    public void SetListViewInteractable(bool interactable)
    {
        foreach (SlotView view in ListView.TraversalActive())
            (view.GetContentView() as GachaItemView).SetInteractable(interactable);
    }
    
    // 两个button锁住无法交互
    // ListView所有item变Free
    // ListView所有item翻到背面
    // ListView 洗牌
    // Model中随机化牌序
    // ListView 排列
    public Tween Idle2Picking()
        => DOTween.Sequence()
            .AppendCallback(RefreshBuyButton)
            .AppendCallback(() => ExitButton.SetStateToActiveIf(false))
            .AppendCallback(() => SetListViewInteractable(false))
            .AppendCallback(() =>
            {
                foreach (SlotView view in ListView.TraversalActive())
                    (view.GetContentView() as GachaItemView).GetBehaviour<FlipBehaviour>().SetFlipped(true);
            })
            .AppendInterval(0.3f)
            .AppendCallback(() =>
            {
                HLayout.spacing = -170;
                ListView.RefreshPivotsAsync();
            })
            .AppendInterval(0.15f)
            .AppendCallback(() =>
            {
                HLayout.spacing = -130;
                ListView.RefreshPivotsAsync();
            })
            .AppendInterval(0.15f)
            .AppendCallback(() =>
            {
                HLayout.spacing = -170;
                ListView.RefreshPivotsAsync();
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
                GachaCell cell = cellAdapter.AsCell() as GachaCell;
                cell.Shuffle();
                ListView.Refresh();
                HLayout.spacing = -50;
                ListView.RefreshPivotsAsync();
            })
            .AppendInterval(0.3f)
            .AppendCallback(() => SetListViewInteractable(true));

    // 被抽到的牌获得Staging
    // ListView 排列
    // ListView 翻回正面
    // 两个Button允许交互
    public Tween Picking2Idle()
    {
        Sequence seq = DOTween.Sequence();
        
        // 1. 被抽到的牌获得Staging (这个在 GachaStaging 中已经处理)
        // 这里只需要等待 Staging 完成
        seq.AppendCallback(() => SetListViewInteractable(false))
            .AppendInterval(0.2f)
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
                RefreshBuyButton();
                ExitButton.SetStateToActiveIf(true);
                HLayout.spacing = 20;
                ListView.RefreshPivotsAsync();
            })
            .AppendInterval(0.3f)
            .AppendCallback(() => SetListViewInteractable(true));
        
        return seq;
    }

    public override Tween EnterHide()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));
}
