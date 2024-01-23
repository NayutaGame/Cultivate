
using TMPro;
using UnityEngine;

public class MarkCursorView : MonoBehaviour
{
    [SerializeField] private RectTransform RectTransform;
    public Vector3 GetLocalPosition() => RectTransform.localPosition;
    public void SetLocalPosition(Vector3 localPosition) => RectTransform.localPosition = localPosition;

    [SerializeField] private TMP_Text Text;
    public void SetText(string text) => Text.text = text;
}
