
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraBehaviourNextShowingJingJie : ExtraBehaviour
{
    [SerializeField] private SkillCardView _skillView;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.BeginDragNeuron.Join(ResetJingJie);
        ib.PointerExitNeuron.Join(ResetJingJie);
        ib.RightClickNeuron.Join(NextJingJie);
    }
    
    private void ResetJingJie(InteractBehaviour ib, PointerEventData d)
    {
        ISkillModel skill = _skillView.Get<ISkillModel>();
        if (skill == null)
            return;
        _skillView.Refresh();
    }
    
    private void NextJingJie(InteractBehaviour ib, PointerEventData d)
    {
        ISkillModel skill = _skillView.Get<ISkillModel>();
        if (skill == null)
            return;
        _skillView.SetShowingJingJie(skill.NextJingJie(_skillView.GetShowingJingJie()));
    }
}
