
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class FormationGroupBarView : LegacySimpleView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public virtual bool IsSelected() => _selected;
    public virtual void SetSelected(bool selected)
    {
        _selected = selected;
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    }

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        IFormationModel formation = Get<IFormationModel>();

        bool formationIsNull = formation == null;
        gameObject.SetActive(!formationIsNull);
        if (formationIsNull)
            return;

        NameText.text = formation.GetName();
    }
}
