
using UnityEngine;
using UnityEngine.EventSystems;

public class ComplexView : MonoBehaviour
{
    [SerializeField] public AddressBehaviour AddressBehaviour;
    [SerializeField] protected ItemBehaviour ItemBehaviour;
    [SerializeField] protected InteractBehaviour InteractBehaviour;
    [SerializeField] protected AnimateBehaviour AnimateBehaviour;
    [SerializeField] protected PivotBehaviour PivotBehaviour;
    [SerializeField] protected SelectBehaviour SelectBehaviour;

    public RectTransform GetDisplayTransform()
        => AddressBehaviour.RectTransform;

    public void SetDisplayTransform(RectTransform pivot)
    {
        AddressBehaviour.RectTransform.position = pivot.position;
        AddressBehaviour.RectTransform.localScale = pivot.localScale;
    }

    private void OnEnable()
    {
        if (AnimateBehaviour != null)
        {
            InteractBehaviour.PointerEnterEvent += AnimateBehaviour.Hover;
            InteractBehaviour.PointerExitEvent += AnimateBehaviour.Unhover;
        }
    }

    private void OnDisable()
    {
        if (AnimateBehaviour != null)
        {
            InteractBehaviour.PointerEnterEvent -= AnimateBehaviour.Hover;
            InteractBehaviour.PointerExitEvent -= AnimateBehaviour.Unhover;
        }
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        // if (eventData.dragging) return;
        //
        // AudioManager.Play("CardHover");
        //
        // ComplexView.AnimateBehaviour.SetPivot(ComplexView.PivotBehaviour.HoverPivot);
        //
        // CanvasManager.Instance.SkillAnnotation.SetAddress(GetSkillAddress());
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        // if (eventData.dragging) return;
        //
        // ComplexView.AnimateBehaviour.SetPivot(ComplexView.PivotBehaviour.IdlePivot);
        //
        // CanvasManager.Instance.SkillAnnotation.SetAddress(null);
    }
}
