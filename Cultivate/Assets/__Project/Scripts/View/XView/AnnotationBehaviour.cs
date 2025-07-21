
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private float FirstCounter = 0f;
    [SerializeField] private float SecondCounter = 0f;
    [SerializeField] private string AnnotationAddress;
    [SerializeField] private AnnotationViewType AnnotationViewType;
    
    private LegacyAnnotationView _annotationView;
    public LegacyAnnotationView GetAnnotationView() => _annotationView;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _annotationView = new Address(AnnotationAddress).Get<LegacyAnnotationView>();
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            _ib.PointerEnterNeuron.Remove(TryShowAnnotation);
            _ib.PointerExitNeuron.Remove(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
            _ib.BeginDragNeuron.Remove(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
        }

        _ib = ib;
        if (_ib != null)
        {
            _ib.PointerEnterNeuron.Join(TryShowAnnotation);
            _ib.PointerExitNeuron.Join(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
            _ib.BeginDragNeuron.Join(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
        }
    }

    public void TryShowAnnotation(InteractBehaviour ib, PointerEventData d)
    {
        AnnotationDetails annotationDetails = new AnnotationDetails(
            AnnotationViewType,
            null,
            ib.GetAddress(),
            FirstCounter,
            SecondCounter,
            new RectTransformAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView())));
        CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
    }

    private RectTransform GetAlignRectTransform(XView view)
    {
        if (view is SlotView slotView)
            return slotView.GetContentView().GetRect();
        return view.GetRect();
    }
}
