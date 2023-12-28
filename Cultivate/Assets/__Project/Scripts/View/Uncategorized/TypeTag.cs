
using TMPro;
using UnityEngine;

public class TypeTag : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;

    public void SetText(string text)
    {
        if (text == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        Text.text = text;
    }
}
