
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private float FirstCounter = 0f;
    [SerializeField] private float SecondCounter = 0f;
    [SerializeField] private AnnotationViewType AnnotationViewType;
    [SerializeField] private bool UseRectAlignment = true;

    public Neuron InvokeShowAnnotation = new();
    public Neuron InvokeHideAnnotation = new();

    public override void AwakeFunction()
    {
        base.AwakeFunction();
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
        AnnotationAlignmentDetails alignmentDetails = UseRectAlignment
            ? new ImageAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView()))
            : new MouseAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView()).rect);
        
        AnnotationDetails annotationDetails = new AnnotationDetails(
            AnnotationViewType,
            GetView().GetRect(),
            ib.GetAddress(),
            this,
            FirstCounter,
            SecondCounter,
            alignmentDetails);
        CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
    }

    private RectTransform GetAlignRectTransform(XView view)
    {
        if (view is SlotView slotView)
            return slotView.GetContentView().GetRect();
        return view.GetRect();
    }
}
