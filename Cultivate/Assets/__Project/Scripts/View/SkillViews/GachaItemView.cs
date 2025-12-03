
using DG.Tweening;
using UnityEngine.EventSystems;

public class GachaItemView : XView
{
    public SlotView Front;
    public SlotView Back;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        Front.CheckAwake();
        Back.CheckAwake();
    }

    private void OnEnable()
    {
        Back.GetInteractBehaviour().LeftClickNeuron.Add(Gacha);
    }

    private void OnDisable()
    {
        Back.GetInteractBehaviour().LeftClickNeuron.Remove(Gacha);
    }

    public void SetInteractable(bool interactable)
    {
        Front.GetAnimator().SetState(interactable ? SlotView.IDLE : SlotView.FREE);
        Back.GetAnimator().SetState(interactable ? SlotView.IDLE : SlotView.FREE);
    }

    private void Gacha(InteractBehaviour ib, PointerEventData d)
    {
        Tween handle = DOTween.Sequence()
            .AppendCallback(() => CanvasManager.Instance.RunCanvas.GachaPanel.SetListViewInteractable(false))
            .AppendCallback(() => GetBehaviour<FlipBehaviour>().SetFlipped(false))
            .AppendInterval(0.5f)
            .AppendCallback(() => Get<GachaItem>().GachaProcedure())
            .AppendInterval(0.1f)
            .AppendCallback(() => CanvasManager.Instance.RunCanvas.GachaPanel.GetAnimator().SetStateAsync(Panel.IDLE));
        handle.SetAutoKill().Restart();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        Front.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();
        Front.Refresh();
    }
}
