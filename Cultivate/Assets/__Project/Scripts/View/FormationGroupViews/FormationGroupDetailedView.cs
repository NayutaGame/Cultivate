
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SelectionBase]
public class FormationGroupDetailedView : SimpleView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private ListView FormationListView;

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        FormationGroupEntry formationGroup = Get<FormationGroupEntry>();
        if (formationGroup == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        SetName(formationGroup.Name);
        SetSubFormations();
    }

    public virtual void SetName(string s)
    {
        NameText.text = s;
    }

    public virtual void SetSubFormations()
    {
        FormationListView.SetAddress(GetAddress().Append(".SubFormations"));
        FormationListView.Refresh();
    }
}
