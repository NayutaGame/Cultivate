
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscoverSkillPanel : Panel
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DetailedText;
    [SerializeField] private ListView SkillViews;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        SkillViews.SetAddress(_address.Append(".Skills"));
        SkillViews.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillViews.LeftClickNeuron.Join(TrySelectOption);
    }

    public override void Refresh()
    {
        base.Refresh();

        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        TitleText.text = d.GetTitleText();
        DetailedText.text = d.GetDetailedText();

        // bool active = i < d.GetSkillCount() && !RunManager.Instance.Environment.Map.Selecting;
        //
        // for (int i = 0; i < SkillViews.Length; i++)
        // {
        //     bool active = i < d.GetSkillCount() && !RunManager.Instance.Environment.Map.Selecting;
        //     SkillViews[i].gameObject.SetActive(active);
        //     if(!active)
        //         continue;
        //
        //     SkillViews[i].SetAddress(_address.Append($".Skills#{i}"));
        //     SkillViews[i].Refresh();
        // }
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void TrySelectOption(InteractBehaviour ib, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        SkillDescriptor skill = ib.GetSimpleView().Get<SkillDescriptor>();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        CanvasManager.Instance.SkillAnnotation.PointerExit(ib, eventData);
    }
}
