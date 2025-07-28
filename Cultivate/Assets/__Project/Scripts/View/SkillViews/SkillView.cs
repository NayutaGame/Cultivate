
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : XView
{
    [SerializeField] private Image Illustration;
    [SerializeField] private CostView CostView;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    
    private JingJie _showingJingJie;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        CostView.SetAddress(GetAddress());
    }

    public override void Refresh()
    {
        base.Refresh();

        AnnotatableSkill skill = Get<AnnotatableSkill>();
        SetShowingJingJie(skill.GetJingJie());
    }

    public JingJie GetShowingJingJie()
        => _showingJingJie;

    public void SetShowingJingJie(JingJie jingJie)
    {
        _showingJingJie = jingJie;
        // CostView不再需要单独Refresh一次
        CostView.SetShowingJingJie(_showingJingJie);

        AnnotatableSkill skill = Get<AnnotatableSkill>();
        Illustration.sprite = skill.GetSprite();
        NameText.text = skill.GetName();
        DescriptionText.text = skill.GetDescription(_showingJingJie).GetHighlightedString();
        JingJieImage.sprite = skill.GetJingJieSprite(_showingJingJie);
    }
}
