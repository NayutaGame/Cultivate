
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ArcDefinitionView : MonoBehaviour
{
    [SerializeField] private RectTransform P1;
    [SerializeField] private RectTransform P2;
    [SerializeField] private RectTransform P3;
    [SerializeField] private RectTransform ImageContainer;
    [SerializeField] private Gradient _colorGradient;
    
    [ReadOnly]
    [SerializeField] public ArcDefinition ArcDefinition;

    private Vector2 _lastP1Position;
    private Vector2 _lastP2Position;
    private Vector2 _lastP3Position;

    private void Update()
    {
        // 只在 Editor 模式下运行，运行时跳过
        if (Application.isPlaying)
            return;
        
        if (!NeedUpdate())
            return;
        
        UpdateArc();
        UpdateItems();
    }

    private bool NeedUpdate()
    {
        // 检查是否有 null
        if (P1 == null || P2 == null || P3 == null)
            return false;
        
        // 检查值是否改变
        return
            P1.anchoredPosition != _lastP1Position ||
            P2.anchoredPosition != _lastP2Position ||
            P3.anchoredPosition != _lastP3Position;
    }

    private void UpdateArc()
    {
        Vector2 p1Pos = P1.anchoredPosition;
        Vector2 p2Pos = P2.anchoredPosition;
        Vector2 p3Pos = P3.anchoredPosition;

        // 更新位置记录
        _lastP1Position = p1Pos;
        _lastP2Position = p2Pos;
        _lastP3Position = p3Pos;

        // 根据三个点计算 ArcDefinition
        ArcDefinition = ArcDefinition.FromThreePoints(p1Pos, p2Pos, p3Pos);
    }

    private void UpdateItems()
    {
        if (ImageContainer == null || ArcDefinition == null)
            return;

        int childCount = ImageContainer.childCount;
        
        if (childCount == 0)
            return;

        // 更新每个子对象的位置和颜色
        for (int i = 0; i < childCount; i++)
        {
            RectTransform childRect = ImageContainer.GetChild(i) as RectTransform;
            if (childRect == null)
                continue;

            // 计算 t 值：第一个为 0，最后一个为 1，中间线性插值
            float t = childCount > 1 ? (float)i / (childCount - 1) : 0f;

            // 设置位置：根据 arc 的 EvaluatePoint
            Vector2 arcPosition = ArcDefinition.EvaluatePoint(t);
            childRect.anchoredPosition = arcPosition;

            // 设置颜色：根据 gradient 的 Evaluate
            Image childImage = childRect.GetComponent<Image>();
            if (childImage == null || _colorGradient == null)
                continue;
            
            childImage.color = _colorGradient.Evaluate(t);
        }
    }
}
