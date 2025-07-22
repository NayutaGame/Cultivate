
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAnnotationView : AnnotationView
{
    [SerializeField] private SkillView SkillView;
    [SerializeField] private Button[] JingJieButtons;
    [SerializeField] private ListView Tags;
    
    [SerializeField] private TMP_Text TriviaText;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
        => SkillView.GetRect().position - GetRect().position;
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        AnnotationDetails d = Get<AnnotationDetails>();
        SkillView.SetAddress(d.Address);
        for (int i = 0; i < JingJieButtons.Length; i++)
        {
            int showingJingJie = i;
            JingJieButtons[i].onClick.RemoveAllListeners();
            JingJieButtons[i].onClick.AddListener(() => SetShowingJingJie(showingJingJie));
        }
        Tags.SetAddress(d.Address.Append(".TagComposite.TagList"));
    }

    private void SetShowingJingJie(int showingJingJie)
        => SkillView.SetShowingJingJie(showingJingJie);

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableSkill skill = d.Address.Get<AnnotatableSkill>();

        SetJingJieButtons(skill);
        SkillView.Refresh();
        Tags.Refresh();

        // SetTrivia(skill.GetTrivia());
    }

    private void SetJingJieButtons(AnnotatableSkill skill)
    {
        int lowestJingJie = skill.GetLowestJingJie();
        int highestJingJie = skill.GetHighestJingJie();

        for (int i = 0; i < JingJieButtons.Length; i++)
        {
            bool inRange = lowestJingJie <= i && i <= highestJingJie;
            JingJieButtons[i].interactable = inRange;
        }
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }
}
