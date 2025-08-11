
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAnnotationView : AnnotationView
{
    [SerializeField] private SkillView SkillView;
    [SerializeField] private JingJieBit[] JingJieBits;
    [SerializeField] private JingJieView JingJieView;
    [SerializeField] private ListView Tags;
    
    [SerializeField] private TMP_Text TriviaText;
    [SerializeField] private XView PackAnnotationProvider;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
        => SkillView.GetRect().position - GetRect().position;
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        AnnotationDetails d = Get<AnnotationDetails>();
        SkillView.SetAddress(d.Address);
        TagsSetAddress(d.Address);
        PackAnnotationProviderSetAddress(d.Address);
    }

    private void TagsSetAddress(Address skillAddress)
    {
        Address tagsAddress = skillAddress.Append(".TagComposite.TagList");
        IListModel tags = tagsAddress.Get<IListModel>();
        bool hasTags = tags != null;
        Tags.gameObject.SetActive(hasTags);
        Tags.SetAddress(hasTags ? tagsAddress : null);
    }

    private void PackAnnotationProviderSetAddress(Address skillAddress)
    {
        Address packAddress = skillAddress.Append(".PackEntry");
        AnnotatablePack pack = packAddress.Get<AnnotatablePack>();
        bool hasPack = pack != null;
        PackAnnotationProvider.gameObject.SetActive(hasPack);
        PackAnnotationProvider.SetAddress(hasPack ? packAddress : null);
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillView.Refresh();
        UpdateJingJieSlider();
        Tags.Sync();

        // SetTrivia(skill.GetTrivia());
    }

    private void UpdateJingJieSlider()
    {
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableSkill skill = d.Address.Get<AnnotatableSkill>();
        
        int lowestJingJie = skill.GetLowestJingJie();
        int highestJingJie = skill.GetHighestJingJie();

        for (int i = 0; i < JingJieBits.Length; i++)
        {
            int showingJingJie = i;
            bool inRange = lowestJingJie <= i && i <= highestJingJie;

            bool disabled = !inRange;
            if (disabled)
            {
                JingJieBits[i].PropagatePointerEnter._onPointerEnter = null;
                JingJieBits[i].Image.sprite = CanvasManager.Instance.JingJieSliderSprites[2];
                continue;
            }
            
            bool active = SkillView.GetShowingJingJie() == i;
            if (active)
            {
                JingJieBits[i].PropagatePointerEnter._onPointerEnter = null;
                JingJieBits[i].Image.sprite = CanvasManager.Instance.JingJieSliderSprites[0];
                continue;
            }
            
            JingJieBits[i].PropagatePointerEnter._onPointerEnter = d => JingJieIsHover(showingJingJie);
            JingJieBits[i].Image.sprite = CanvasManager.Instance.JingJieSliderSprites[1];
        }

        int j = SkillView.GetShowingJingJie();
        Address jingJieAddress = new Address($"Encyclopedia.JingJieCategory.List#{j}");
        JingJieView.SetAddress(jingJieAddress);
        JingJieView.Refresh();
    }

    private void JingJieIsHover(int showingJingJie)
    {
        SkillView.SetShowingJingJie(showingJingJie);
        UpdateJingJieSlider();
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }

    public override void DidAlign()
    {
        base.DidAlign();
        Tags.RefreshPivots();
    }
}
