
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechView : AddressBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text CountText;

    public override void Refresh()
    {
        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Mech mech = Get<Mech>();
        if (mech == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        SetMechType(mech.GetMechType());
        SetCount(mech.Count);
    }

    private void SetMechType(MechType mechType)
    {
        NameText.text = mechType.ToString();
    }

    private void SetCount(int count)
    {
        CountText.text = count.ToString();
    }
}
