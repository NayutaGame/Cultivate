
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private AnnotationOpenDetails OpenDetails;
    [SerializeField] private AnnotationViewType AnnotationViewType;
    [SerializeField] private bool UseRectAlignment = true;

    public Neuron InvokeShowAnnotation = new();
    public Neuron InvokeHideAnnotation = new();

    private (float firstCounter, float secondCounter) GetCounterValues()
    {
        return OpenDetails switch
        {
            AnnotationOpenDetails.Instant => (0f, 0f),
            AnnotationOpenDetails.VeryShortInterval => (0.2f, 0f),
            AnnotationOpenDetails.LongInterval => (0.5f, 2f),
            AnnotationOpenDetails.RightClick => (0f, 0f),
            _ => (0f, 0f)
        };
    }

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            // 根据OpenDetails移除之前注册的事件
            switch (OpenDetails)
            {
                case AnnotationOpenDetails.Instant:
                    _ib.PointerEnterNeuron.Remove(TryShowAnnotation);
                    break;
                case AnnotationOpenDetails.VeryShortInterval:
                    _ib.PointerEnterNeuron.Remove(TryShowAnnotation);
                    break;
                case AnnotationOpenDetails.LongInterval:
                    _ib.PointerEnterNeuron.Remove(TryShowAnnotation);
                    break;
                case AnnotationOpenDetails.RightClick:
                    _ib.RightClickNeuron.Remove(ShowAnnotation);
                    break;
            }
            
            _ib.PointerExitNeuron.Remove(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
            _ib.BeginDragNeuron.Remove(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
        }

        _ib = ib;
        if (_ib != null)
        {
            // 根据OpenDetails注册相应的事件
            switch (OpenDetails)
            {
                case AnnotationOpenDetails.Instant:
                    _ib.PointerEnterNeuron.Join(TryShowAnnotation);
                    break;
                case AnnotationOpenDetails.VeryShortInterval:
                    _ib.PointerEnterNeuron.Join(TryShowAnnotation);
                    break;
                case AnnotationOpenDetails.LongInterval:
                    _ib.PointerEnterNeuron.Join(TryShowAnnotation);
                    break;
                case AnnotationOpenDetails.RightClick:
                    _ib.RightClickNeuron.Join(ShowAnnotation);
                    break;
            }
            
            _ib.PointerExitNeuron.Join(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
            _ib.BeginDragNeuron.Join(CanvasManager.Instance.AnnotationManager.StopShowAnnotation);
        }
    }

    public void ShowAnnotation(InteractBehaviour ib, PointerEventData d)
    {
        AnnotationAlignmentDetails alignmentDetails = UseRectAlignment
            ? new ImageAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView()))
            : new MouseAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView()).rect);
        
        AnnotationDetails annotationDetails = new AnnotationDetails(
            AnnotationViewType,
            GetView().GetRect(),
            ib.GetAddress(),
            this,
            0,
            0,
            alignmentDetails);
        CanvasManager.Instance.AnnotationManager.TryShowAnnotation(annotationDetails);
    }

    public void TryShowAnnotation(InteractBehaviour ib, PointerEventData d)
    {
        AnnotationAlignmentDetails alignmentDetails = UseRectAlignment
            ? new ImageAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView()))
            : new MouseAnnotationAlignmentDetails(GetAlignRectTransform(ib.GetView()).rect);
        
        var (firstCounter, secondCounter) = GetCounterValues();
        
        AnnotationDetails annotationDetails = new AnnotationDetails(
            AnnotationViewType,
            GetView().GetRect(),
            ib.GetAddress(),
            this,
            firstCounter,
            secondCounter,
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
