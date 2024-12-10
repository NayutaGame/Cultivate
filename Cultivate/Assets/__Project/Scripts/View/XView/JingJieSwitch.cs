
using UnityEngine;
using UnityEngine.EventSystems;

public class JingJieSwitch : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private SkillView _skillView;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        BindInteractBehaviour();
    }

    private void BindInteractBehaviour()
    {
        if (_ib == null)
            return;

        _ib.BeginDragNeuron.Join(ResetJingJie);
        _ib.PointerExitNeuron.Join(ResetJingJie);
        _ib.RightClickNeuron.Join(NextJingJie);
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
