
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class XButton : MonoBehaviour
{
    [SerializeField] public Button _button;
    [SerializeField] public PropagatePointerEnter _propagatePointerEnter;

    // private void Awake()
    // {
    //     _button.onClick.AddListener(ButtonIsClicked);
    //     _propagatePointerEnter._onPointerEnter += ButtonIsHovered;
    // }
    //
    // private void ButtonIsClicked()
    // {
    //     Debug.Log("Button is clicked");
    // }
    //
    // private void ButtonIsHovered(PointerEventData d)
    // {
    //     Debug.Log("Button is hovered");
    // }
}