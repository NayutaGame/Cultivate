
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;

public class DiscoverSkillPanel : Panel
{
    public TMP_Text DetailedText;
    public RunSkillView[] SkillViews;

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

        InteractDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, (v, d) => TrySelectOption(v, d));
    }

    public override void Refresh()
    {
        base.Refresh();

        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

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
        CanvasManager.Instance.SkillPreview.SetAddress(null);
        CanvasManager.Instance.SkillPreview.Refresh();
        return true;
    }
}
