
using System;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropagateLink : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField] private TMP_Text Text;
    [SerializeField] private Image Image;
    private Neuron<TMP_Text, TMP_LinkInfo> _neuron = new();

    public void RegisterCallback(Action<TMP_Text, TMP_LinkInfo> func)
    {
        _neuron.Join(func);
        bool acceptRaycast = _neuron.Count > 0;
        Image.raycastTarget = acceptRaycast;
    }

    public void UnregisterCallback(Action<TMP_Text, TMP_LinkInfo> func)
    {
        _neuron.Remove(func);
        bool acceptRaycast = _neuron.Count > 0;
        Image.raycastTarget = acceptRaycast;
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(Text, eventData.position, CanvasManager.Instance.GetCamera());
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = Text.textInfo.linkInfo[linkIndex];
            _neuron.Invoke(Text, linkInfo);
            // Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}