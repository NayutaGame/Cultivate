
using UnityEngine;
using UnityEngine.EventSystems;

public class NextShowingJingJieBehaviour : XBehaviour
{
    [SerializeField] private SkillView _skillView;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        BindInteractBehaviour();
    }

    private void BindInteractBehaviour()
    {
        InteractBehaviour ib = GetInteractBehaviour();
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
