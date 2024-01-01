
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;

public class DiscoverSkillPanel : Panel
{
    public TMP_Text TitleText;
    public TMP_Text DetailedText;
    public SkillView[] SkillViews;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        SkillViews.Do(v =>
        {
            InteractBehaviour ib = v.GetComponent<InteractBehaviour>();
            if (ib == null)
                return;

            ib.PointerEnterNeuron.Set(
                CanvasManager.Instance.SkillAnnotation.SetAddressFromIB,
                PointerEnter);

            ib.PointerExitNeuron.Set(
                CanvasManager.Instance.SkillAnnotation.SetAddressToNull,
                PointerExit);

            ib.PointerMoveNeuron.Set(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
            ib.LeftClickNeuron.Set(TrySelectOption);
        });
    }

    public override void Refresh()
    {
        base.Refresh();

        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        TitleText.text = d.GetTitleText();
        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < SkillViews.Length; i++)
        {
            bool active = i < d.GetSkillCount() && !RunManager.Instance.Environment.Map.Selecting;
            SkillViews[i].gameObject.SetActive(active);
            if(!active)
                continue;

            SkillViews[i].SetAddress(_address.Append($".Skills#{i}"));
            SkillViews[i].Refresh();
        }
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData eventData)
    {
        AudioManager.Play("CardHover");
        ib.ComplexView.AnimateBehaviour.SetPivot(ib.ComplexView.PivotBehaviour.HoverPivot);
    }

    private void PointerExit(InteractBehaviour ib, PointerEventData eventData)
    {
        ib.ComplexView.AnimateBehaviour.SetPivot(ib.ComplexView.PivotBehaviour.IdlePivot);
    }

    private void TrySelectOption(InteractBehaviour ib, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        RunSkill skill = ib.ComplexView.AddressBehaviour.Get<RunSkill>();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        CanvasManager.Instance.SkillAnnotation.SetAddressToNull(ib, eventData);
    }
}
