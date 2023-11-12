using TMPro;
using UnityEngine;

public class TextHint : MonoBehaviour
{
    [SerializeField] protected RectTransform _rectTransform;
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

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }
}
