
using System;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        if (TryInterpretAsCharacter(linkId, criticalCharInfo, alignRect, out annotationDetails))
        {
            CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
            return;
        }

        if (TryInterpretAsSkill(linkId, criticalCharInfo, alignRect, out annotationDetails))
        {
            CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
            return;
        }

        if (TryInterpretAsBuff(linkId, criticalCharInfo, alignRect, out annotationDetails))
        {
            CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
            return;
        }
        
        if (TryInterpretAsKeyword(linkId, criticalCharInfo, alignRect, out annotationDetails))
        {
            CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
            return;
        }
        
        if (TryInterpretAsTag(linkId, criticalCharInfo, alignRect, out annotationDetails))
        {
            CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
            return;
        }
        
        if (annotationDetails == null)
            return;
    }

    private bool TryInterpretAsTag(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect, out AnnotationDetails annotationDetails)
    {
        if (!Encyclopedia.TagCategory.ContainsKey(linkId))
        {
            annotationDetails = null;
            return false;
        }

        TagEntry tagEntry = TagEntry.FromName(linkId);
        
        int characterIndex = tagEntry.GetName().IndexOf(criticalCharInfo.character);
        annotationDetails = new AnnotationDetails(
            AnnotationViewType.TagAnnotation,
            GetComponent<RectTransform>(),
            new Address($"Encyclopedia.TagCategory.Dict.{linkId}"),
            0,
            0,
            new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
        return true;
    }

    private bool TryInterpretAsKeyword(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect, out AnnotationDetails annotationDetails)
    {
        if (!Encyclopedia.KeywordCategory.ContainsKey(linkId))
        {
            annotationDetails = null;
            return false;
        }
        
        KeywordEntry keywordEntry = KeywordEntry.FromName(linkId);
        
        int characterIndex = keywordEntry.GetName().IndexOf(criticalCharInfo.character);
        annotationDetails = new AnnotationDetails(
            AnnotationViewType.TextAnnotation,
            GetComponent<RectTransform>(),
            new Address($"Encyclopedia.KeywordCategory.Dict.{linkId}"),
            0,
            0,
            new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
        return true;
    }

    private bool TryInterpretAsBuff(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect, out AnnotationDetails annotationDetails)
    {
        if (!Encyclopedia.BuffCategory.ContainsKey(linkId))
        {
            annotationDetails = null;
            return false;
        }
        
        BuffEntry buffEntry = linkId;
        
        int characterIndex = buffEntry.GetName().IndexOf(criticalCharInfo.character);
        annotationDetails = new AnnotationDetails(
            AnnotationViewType.BuffAnnotation,
            GetComponent<RectTransform>(),
            new Address($"Encyclopedia.BuffCategory.Dict.{linkId}"),
            0,
            0,
            new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
        return true;
    }

    private bool TryInterpretAsSkill(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect, out AnnotationDetails annotationDetails)
    {
        if (!Encyclopedia.SkillCategory.ContainsName(linkId))
        {
            annotationDetails = null;
            return false;
        }

        SkillEntry skillEntry = Encyclopedia.SkillCategory.FromName(linkId);
        string skillId = skillEntry.GetId();
        
        int characterIndex = skillEntry.GetName().IndexOf(criticalCharInfo.character);
        annotationDetails = new AnnotationDetails(
            AnnotationViewType.SkillAnnotation,
            GetComponent<RectTransform>(),
            new Address($"Encyclopedia.SkillCategory.Dict.{skillId}"),
            0,
            0,
            new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
        return true;
    }

    private bool TryInterpretAsCharacter(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect, out AnnotationDetails annotationDetails)
    {
        if (!Encyclopedia.CharacterCategory.ContainsKey(linkId))
        {
            annotationDetails = null;
            return false;
        }
        
        CharacterEntry characterEntry = linkId;
        
        int characterIndex = characterEntry.GetName().IndexOf(criticalCharInfo.character);
        annotationDetails = new AnnotationDetails(
            AnnotationViewType.CharacterAnnotation,
            GetComponent<RectTransform>(),
            new Address($"Encyclopedia.CharacterCategory.Dict.{linkId}"),
            0,
            0,
            new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
        return true;
    }

    // private bool TryInterpretAsJingJie(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect, out AnnotationDetails annotationDetails)
    // {
    //     if (!JingJie.ContainsName(linkId))
    //     {
    //         annotationDetails = null;
    //         return false;
    //     }
    //
    //     JingJie jingJie = JingJie.FromName(linkId).Value;
    //     
    //     int characterIndex = jingJie.GetName().IndexOf(criticalCharInfo.character);
    //     annotationDetails = new AnnotationDetails(
    //         AnnotationViewType.JingJieAnnotation,
    //         GetComponent<RectTransform>(),
    //         new Address($"Encyclopedia.CharacterCategory.Dict.{linkId}"),
    //         0,
    //         0,
    //         new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
    //     return true;
    // }
}