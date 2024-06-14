
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
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void TrySelectOption(InteractBehaviour ib, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        SkillDescriptor skill = ib.GetSimpleView().Get<SkillDescriptor>();
        // TODO: Discover Animation
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        PanelS panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
        
        CanvasManager.Instance.SkillAnnotation.PointerExit(ib, eventData);
    }
}
