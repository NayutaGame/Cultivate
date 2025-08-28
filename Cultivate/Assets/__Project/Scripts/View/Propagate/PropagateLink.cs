
using System;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateLink : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField] private TMP_Text Text;
    private Neuron<TMP_Text, TMP_LinkInfo> _neuron = new();

    private void Awake()
    {
        RegisterCallback(LinkCallback);
    }

    public void RegisterCallback(Action<TMP_Text, TMP_LinkInfo> func)
    {
        _neuron.Join(func);
        bool acceptRaycast = _neuron.Count > 0;
        Text.raycastTarget = acceptRaycast;
    }

    public void UnregisterCallback(Action<TMP_Text, TMP_LinkInfo> func)
    {
        _neuron.Remove(func);
        bool acceptRaycast = _neuron.Count > 0;
        Text.raycastTarget = acceptRaycast;
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

    private void LinkCallback(TMP_Text text, TMP_LinkInfo linkInfo)
    {
        int firstCharIndex = linkInfo.linkTextfirstCharacterIndex;
        int lastCharIndex = firstCharIndex + linkInfo.linkTextLength - 1;

        Vector3 mouseWorldPos = CanvasManager.Instance.UI2World(Input.mousePosition);
        
        // 记录最短距离和索引
        float minDist = float.MaxValue;
        int criticalCharIndex = firstCharIndex;

        for (int i = firstCharIndex; i <= lastCharIndex; i++)
        {
            var charInfo = text.textInfo.characterInfo[i];
            // 字符中心点（本地坐标）
            Vector3 charCenter = (charInfo.bottomLeft + charInfo.topRight) * 0.5f;
            // 转为世界坐标
            Vector3 charWorldCenter = text.transform.TransformPoint(charCenter);

            float dist = Vector2.SqrMagnitude(new Vector2(mouseWorldPos.x, mouseWorldPos.y) - new Vector2(charWorldCenter.x, charWorldCenter.y));
            if (dist < minDist)
            {
                minDist = dist;
                criticalCharIndex = i;
            }
        }

        // 获取criticalCharIndex的Rect
        var criticalCharInfo = text.textInfo.characterInfo[criticalCharIndex];
        Vector3 worldBottomLeft = text.transform.TransformPoint(criticalCharInfo.bottomLeft);
        Vector3 worldTopRight = text.transform.TransformPoint(criticalCharInfo.topRight);
        Rect alignRect = new Rect(worldBottomLeft, worldTopRight - worldBottomLeft);

        // Debug.Log($"criticalCharIndex: {criticalCharIndex}, char: {criticalCharInfo.character}, alignRect: {alignRect}");

        string linkId = linkInfo.GetLinkID();

        AnnotationDetails annotationDetails = null;
        foreach (AnnotationManager.CategoryDetails handleDetails in AnnotationManager.CategoryDetailsMappings)
        {
            if (handleDetails.TryInterpret(linkId, criticalCharInfo, alignRect, out annotationDetails))
            {
                annotationDetails.InvokerRectTransform = GetComponent<RectTransform>();
                CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
                return;
            }
        }
        
        if (annotationDetails == null)
            return;
    }
}