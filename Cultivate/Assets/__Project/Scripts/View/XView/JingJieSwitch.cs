
using UnityEngine;
using UnityEngine.EventSystems;

public class JingJieSwitch : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private SkillView _skillView;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            _ib.BeginDragNeuron.Remove(ResetJingJie);
            _ib.PointerExitNeuron.Remove(ResetJingJie);
            _ib.RightClickNeuron.Remove(NextJingJie);
        }

        _ib = ib;
        if (_ib != null)
        {
            _ib.BeginDragNeuron.Join(ResetJingJie);
            _ib.PointerExitNeuron.Join(ResetJingJie);
            _ib.RightClickNeuron.Join(NextJingJie);
        }
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
