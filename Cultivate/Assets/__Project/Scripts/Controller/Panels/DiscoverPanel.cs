
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
    private Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillEvent = new();

    public override void AwakeFunction()
    {
        _address = new Address("Run.Environment.ActivePanel");
        ListView.SetAddress(_address.Append(".Skills"));
        ListView.LeftClickNeuron.Join(PickDiscoveredSkill);
        base.AwakeFunction();
    }
    
    // atomic
    // hide -> idle             显示面板和技能
    // idle -> selected         选择技能，其他技能消失
    // selected -> hide         面板淡出
    // selected -> idle         显示新技能
    
    // composed
    // hide -> idle
    // idle -> selected -> hide
    // idle -> selected -> idle

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for idle, 2 for selected
        Animator animator = new(3, "Discover Panel");
        animator[ANY, HIDE] = EnterHide;
        animator[HIDE, IDLE] = EnterIdle;
        animator[IDLE, SELECTED] = Idle2Selected;
        animator[SELECTED, IDLE] = Selected2Idle;
        animator.SetState(HIDE);
        return animator;
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
        PickDiscoveredSkillEvent.Add(RunManager.Instance.Environment.PickDiscoveredSkillProcedure);
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Add(PickDiscoveredSkillStaging);
    }

    private void OnDisable()
    {
        PickDiscoveredSkillEvent.Remove(RunManager.Instance.Environment.PickDiscoveredSkillProcedure);
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Remove(PickDiscoveredSkillStaging);
    }

    private void PickDiscoveredSkill(InteractBehaviour ib, PointerEventData eventData)
    {
        SkillGhost skillGhost = ib.Get<SkillGhost>();
        int pickedIndex = ListView.IndexFromView(ib.GetView() as SlotView).Value;
        PickDiscoveredSkillDetails details = new(skillGhost, pickedIndex);
        PickDiscoveredSkillEvent.Invoke(details);
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
        
        ListView.TraversalActive().Do(v => v.GetInteractBehaviour().SetInteractable(false));
        
        int pickedIndex = d.PickedIndex;
        SlotView slotView = ListView.ViewFromIndex(pickedIndex);
        RectTransform rect = slotView.GetContentView().GetRect();

        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.AddItem();
        SlotView view = CanvasManager.Instance.RunCanvas.DeckPanel.LatestSkillItem();
        view.SetMoveFromRectToIdle(rect);
        
        slotView.GetAnimator().SetState(0);

        CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueAnimation(GetAnimator().TweenFromSetState(SELECTED));
    }

    public Tween Idle2Selected()
        => DOTween.Sequence();

    public Tween Selected2Idle()
        => DOTween.Sequence();
}
