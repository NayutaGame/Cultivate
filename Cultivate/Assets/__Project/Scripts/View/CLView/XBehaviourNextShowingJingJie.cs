
using UnityEngine;
using UnityEngine.EventSystems;

public class XBehaviourNextShowingJingJie : XBehaviour
{
    [SerializeField] private SkillCardView _skillView;

    public override void Init(XView view)
    {
        base.Init(view);

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.BeginDragNeuron.Join(ResetJingJie);
        ib.PointerExitNeuron.Join(ResetJingJie);
        ib.RightClickNeuron.Join(NextJingJie);
    }
    
    private void ResetJingJie(InteractBehaviour ib, PointerEventData d)
    {
        ISkill skill = _skillView.Get<ISkill>();
        if (skill == null)
            return;
        _skillView.Refresh();
    }
    
    private void NextJingJie(InteractBehaviour ib, PointerEventData d)
    {
        ISkill skill = _skillView.Get<ISkill>();
        if (skill == null)
            return;
        _skillView.SetShowingJingJie(skill.NextJingJie(_skillView.GetShowingJingJie()));
    }
}
