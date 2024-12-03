
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillCardView : LegacySimpleView
{
    private JingJie _showingJingJie;
    private bool _highlight;
    
    [SerializeField] private Image Illustration;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private Image CostIcon;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image EffectImage;

    private Material _outlineMaterial;
    // private Material _dissolveMaterial;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        if (EffectImage != null)
        {
            EffectImage.material = Instantiate(EffectImage.material);
            _outlineMaterial = EffectImage.materialForRendering;
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        ISkill skill = Get<ISkill>();
        SetShowingJingJie(skill.GetJingJie());
    }

    public JingJie GetShowingJingJie()
        => _showingJingJie;

    public void SetShowingJingJie(JingJie jingJie)
    {
        _showingJingJie = jingJie;

        ISkill skill = Get<ISkill>();
        SetSprite(skill.GetSprite());
        SetCostDescription(skill.GetCostDescription(_showingJingJie));
        SetName(skill.GetName());
        SetDescription(skill.GetHighlight(_showingJingJie));
        SetSkillTypeComposite(skill.GetSkillTypeComposite());
        SetJingJieSprite(skill.GetJingJieSprite(_showingJingJie));
    }

    private Tween _highlightHandle;
    private static readonly int OuterOutlineFade = Shader.PropertyToID("_OuterOutlineFade");

    public void SetHighlight(bool highlight)
    {
        _highlight = highlight;
        
        _highlightHandle?.Kill();
        _highlightHandle = DOTween.To(GetOutlineFade, SetOutlineFade, _highlight ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad);
        _highlightHandle.SetAutoKill().Restart();
    }

    private float GetOutlineFade() => _outlineMaterial.GetFloat(OuterOutlineFade);
    private void SetOutlineFade(float value) => _outlineMaterial.SetFloat(OuterOutlineFade, value);

    protected virtual void SetSprite(Sprite sprite)
    {
        Illustration.sprite = sprite;
    }

    protected virtual void SetCostDescription(CostDescription costDescription)
    {
        switch (costDescription.Type)
        {
            case CostDescription.CostType.Empty:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[0];
                CostText.text = "";
                break;
            case CostDescription.CostType.Mana:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[1];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostDescription.CostType.Health:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[2];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostDescription.CostType.Channel:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[3];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostDescription.CostType.Armor:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[4];
                CostText.text = costDescription.Value.ToString();
                break;
        }

        int? i = null;
        switch (costDescription.State)
        {
            case CostResult.CostState.Unwritten:
                i = 0;
                break;
            case CostResult.CostState.Normal:
                i = 1;
                break;
            case CostResult.CostState.Reduced:
                i = 2;
                break;
            case CostResult.CostState.Shortage:
                i = 3;
                break;
        }

        if (i.HasValue)
        {
            CostText.color = CanvasManager.Instance.CostColors[i.Value];
        }
    }

    protected virtual void SetName(string name)
    {
        NameText.text = name;
    }

    protected virtual void SetDescription(string description)
    {
        DescriptionText.text = description;
    }

    protected virtual void SetSkillTypeComposite(SkillTypeComposite skillTypeComposite)
    {
        // List<SkillType> skillTypes = skillTypeComposite.ContainedSkillTypes.FirstN(TypeViews.Length).ToList();
        //
        // for (int i = 0; i < skillTypes.Count; i++)
        // {
        //     TypeViews[i].SetActive(true);
        //     TypeTexts[i].text = skillTypes[i].ToString();
        // }
        //
        // for (int i = skillTypes.Count; i < TypeViews.Length; i++)
        // {
        //     TypeViews[i].SetActive(false);
        // }
    }

    protected virtual void SetJingJieSprite(Sprite jingJieSprite)
    {
        JingJieImage.sprite = jingJieSprite;
    }
}
