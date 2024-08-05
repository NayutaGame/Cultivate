
using TMPro;
using UnityEngine;

public class FormationIconView : SimpleView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text ProgressText;

    public override void Refresh()
    {
        base.Refresh();

        IFormationModel formation = Get<IFormationModel>();
        bool formationIsNull = formation == null;
        gameObject.SetActive(!formationIsNull);
        if (formationIsNull)
            return;

        JingJie? jingJie = formation.GetActivatedJingJie();
        if (jingJie != null)
            NameText.text = $"<style={jingJie.ToString()}>{formation.GetName()}</style>";
        else
            NameText.text = formation.GetName();

        if (ProgressText != null)
        {
            if (formation is RunFormation f)
            {
                int progress = f.GetProgress();
                
                JingJie nextJingJie;
                JingJie? activatedJingJie = f.GetActivatedJingJie();
                JingJie highestJingJie = f.GetEntry().GetFormationGroupEntry().SubFormationEntries[0].GetJingJie();
                if (!activatedJingJie.HasValue)
                {
                    nextJingJie = f.GetLowestJingJie();
                }
                else if (activatedJingJie != highestJingJie)
                {
                    nextJingJie = f.GetIncrementedJingJie(activatedJingJie.Value);
                }
                else
                {
                    nextJingJie = highestJingJie;
                }
                
                int requirement = formation.GetRequirementFromJingJie(nextJingJie);
                ProgressText.text = $"{progress}/{requirement}";
            }
        }
    }
}
