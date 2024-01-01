
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageFormationIconView : LegacyAddressBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

    public override void Refresh()
    {
        base.Refresh();
        Formation e = Get<Formation>();
        if (e == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (NameText != null)
            NameText.text = e.GetName();
    }
}
