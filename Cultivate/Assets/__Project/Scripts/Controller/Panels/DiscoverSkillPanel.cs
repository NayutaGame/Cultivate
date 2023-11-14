
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;

public class DiscoverSkillPanel : CurtainPanel
{
    public TMP_Text TitleText;
    public TMP_Text DetailedText;
    public DiscoverSkillPanelSkillView[] SkillViews;

    private InteractDelegate InteractDelegate;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        ConfigureInteractDelegate();
        SkillViews.Do(v => v.SetDelegate(InteractDelegate));
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new InteractDelegate(1, getId: view => 0);

        InteractDelegate.SetHandle(InteractDelegate.POINTER_ENTER, 0, (v, d) => ((DiscoverSkillPanelSkillView)v).HoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_EXIT, 0, (v, d) => ((DiscoverSkillPanelSkillView)v).UnhoverAnimation(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_MOVE, 0, (v, d) => ((DiscoverSkillPanelSkillView)v).PointerMove(d));
        InteractDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, (v, d) => TrySelectOption(v, d));
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

    public bool TrySelectOption(IInteractable view, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        RunSkill skill = view.Get<RunSkill>();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();
        return true;
    }
}
