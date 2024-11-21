
using TMPro;
using UnityEngine;

[SelectionBase]
public class FormationGroupDetailedView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        FormationListView.SetAddress(GetAddress().Append(".SubFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        FormationGroupEntry formationGroup = Get<FormationGroupEntry>();

        bool formationIsNull = formationGroup == null;
        gameObject.SetActive(!formationIsNull);
        if (formationIsNull)
            return;

        NameText.text = formationGroup.GetName();
        FormationListView.Refresh();
    }
}
