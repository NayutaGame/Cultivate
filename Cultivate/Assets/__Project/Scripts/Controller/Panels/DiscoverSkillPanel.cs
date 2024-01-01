
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;

public class DiscoverSkillPanel : Panel
{
    public TMP_Text TitleText;
    public TMP_Text DetailedText;
    public SkillView[] SkillViews;

    private InteractHandler _interactHandler;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        ConfigureInteractDelegate();
        SkillViews.Do(v => v.GetComponent<InteractBehaviour>()?.SetHandler(_interactHandler));
    }

    private void ConfigureInteractDelegate()
    {
        _interactHandler = new InteractHandler(1, getId: view => 0);

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((DiscoverSkillInteractBehaviour)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((DiscoverSkillInteractBehaviour)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((DiscoverSkillInteractBehaviour)v).PointerMove(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, (v, d) => TrySelectOption(v, d));
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

    public bool TrySelectOption(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        RunSkill skill = interactBehaviour.ComplexView.AddressBehaviour.Get<RunSkill>();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();
        return true;
    }
}
