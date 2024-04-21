
using TMPro;
using UnityEngine;

public class FormationIconView : SimpleView
{
    [SerializeField] private TMP_Text NameText;

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
    }
}
