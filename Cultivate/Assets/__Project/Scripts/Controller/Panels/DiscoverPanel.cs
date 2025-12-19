
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscoverPanel : Panel
{
    public static readonly int SELECTED = 2;
    
    [SerializeField] public ListView ListView;
    
    private Address _address;

    public override void AwakeFunction()
    {
        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Skills"));
        ListView.NeuronBundle.LeftClickNeuron.Join(PickDiscoveredSkill);
        base.AwakeFunction();
    }

    private void RefreshInfo()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        DiscoverCell d = cellAdapter.AsCell() as DiscoverCell;

        // TitleText.text = d.GetTitleText();
        // DescriptionText.text = d.GetDescriptionText();
    }
    
    private void OnEnable()
    {
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Add(PickDiscoveredSkillStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Remove(PickDiscoveredSkillStaging);
    }

    private void PickDiscoveredSkill(InteractBehaviour ib, PointerEventData eventData)
    {
        SkillGhost skillGhost = ib.Get<SkillGhost>();
        int pickedIndex = ListView.IndexFromView(ib.GetView() as SlotView).Value;
        PickDiscoveredSkillDetails details = new(skillGhost, pickedIndex);
        RunManager.Instance.Environment.PickDiscoveredSkillProcedure(details);
        CanvasManager.Instance.AnnotationManager.StopShowAnnotation();
    }
    
    public void PickDiscoveredSkillStaging(PickDiscoveredSkillDetails d)
    {
        // ListView中都设置不可访问
        // 卡牌强调
        // 卡牌飞入手中
        // 切换成为Hide状态
        
        // 关上幕布
        // ListView设为正常
        
        SlotView discoverSlot = ListView.ViewFromIndex(d.PickedIndex);
        
        Configuration initial = Configuration.FromRect(discoverSlot.GetContentView().GetRect());
        initial.Scale *= 1f / 0.9375f / 0.95f;

        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.AddItem();
        SlotView handSlot = CanvasManager.Instance.RunCanvas.DeckPanel.LatestSkillItem();

        handSlot.GetAnimator().SetState(SlotView.FREE);
        
        ListView.RemoveItemAt(d.PickedIndex);

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => ListView.TraversalActive().Do(slotView => slotView.GetAnimator().SetState(SlotView.FREE)));
        seq.AppendCallback(() => handSlot.GoToConfiguration(initial, false));
        seq.AppendCallback(() => handSlot.GetAnimator().SetStateAsync(SlotView.IDLE));
        seq.AppendInterval(0.5f);
        CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueAnimation(seq);
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE));

    public override Tween EnterHide()
        => DOTween.Sequence()
            .AppendCallback(() => ListView.TraversalActive().Do(slotView => slotView.GetAnimator().SetState(SlotView.IDLE)))
            .AppendCallback(() => gameObject.SetActive(false));
}
