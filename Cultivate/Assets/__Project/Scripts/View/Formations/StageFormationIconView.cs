
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageFormationIconView : SimpleView
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

        NameText.text = formation.GetName();
    }
}
