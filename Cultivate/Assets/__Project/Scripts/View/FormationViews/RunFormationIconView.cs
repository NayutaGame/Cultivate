
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunFormationIconView : LegacyAddressBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

    public override void Refresh()
    {
        base.Refresh();
        FormationEntry e = Get<FormationEntry>();
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

// TODO: combine with StageFormationIconView into FormationIconView
