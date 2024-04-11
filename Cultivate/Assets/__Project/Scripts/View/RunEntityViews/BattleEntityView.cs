
using TMPro;

public class BattleEntityView : SimpleView
{
    public TMP_Text NameText;
    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FormationListView.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        EntityModel entity = Get<EntityModel>();

        NameText.text = $"{entity.GetJingJie()} {entity.GetEntry().GetName()}";

        FieldView.Refresh();
        FormationListView.Refresh();
    }
}
