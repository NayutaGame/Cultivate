
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
        
        SkillView.PropagateLink.RegisterCallback(LinkCallback);
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

    private void LinkCallback(TMP_Text text, TMP_LinkInfo linkInfo)
    {
        int firstCharIndex = linkInfo.linkTextfirstCharacterIndex;
        int lastCharIndex = firstCharIndex + linkInfo.linkTextLength - 1;

        Vector3 mouseWorldPos = CanvasManager.Instance.UI2World(Input.mousePosition);
        
        // 记录最短距离和索引
        float minDist = float.MaxValue;
        int criticalCharIndex = firstCharIndex;

        for (int i = firstCharIndex; i <= lastCharIndex; i++)
        {
            var charInfo = text.textInfo.characterInfo[i];
            // 字符中心点（本地坐标）
            Vector3 charCenter = (charInfo.bottomLeft + charInfo.topRight) * 0.5f;
            // 转为世界坐标
            Vector3 charWorldCenter = text.transform.TransformPoint(charCenter);

            float dist = Vector2.SqrMagnitude(new Vector2(mouseWorldPos.x, mouseWorldPos.y) - new Vector2(charWorldCenter.x, charWorldCenter.y));
            if (dist < minDist)
            {
                minDist = dist;
                criticalCharIndex = i;
            }
        }

        // 获取criticalCharIndex的Rect
        var criticalCharInfo = text.textInfo.characterInfo[criticalCharIndex];
        Vector3 worldBottomLeft = text.transform.TransformPoint(criticalCharInfo.bottomLeft);
        Vector3 worldTopRight = text.transform.TransformPoint(criticalCharInfo.topRight);
        Rect alignRect = new Rect(worldBottomLeft, worldTopRight - worldBottomLeft);

        // Debug.Log($"criticalCharIndex: {criticalCharIndex}, char: {criticalCharInfo.character}, alignRect: {alignRect}");

        string linkId = linkInfo.GetLinkID();
        KeywordEntry keywordEntry = KeywordEntry.FromName(linkId);
        if (keywordEntry != null)
        {
            // case keyword
            AnnotationDetails parentD = Get<AnnotationDetails>();
            int characterIndex = keywordEntry.GetName().IndexOf(criticalCharInfo.character);
            AnnotationDetails annotationDetails = new AnnotationDetails(
                AnnotationViewType.TextAnnotation,
                parentD,
                new Address($"Encyclopedia.KeywordCategory.Dict.{linkId}"),
                0,
                0,
                new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
            CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
            return;
        }


        // case buff

    }
}
