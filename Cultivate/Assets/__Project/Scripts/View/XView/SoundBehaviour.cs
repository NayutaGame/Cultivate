
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SoundBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private string HoverAudioId;
    [SerializeField] private string LeftClickAudioId;
    [SerializeField] private string RightClickAudioId;

    private AudioEntry _hoverAudioEntry;
    private AudioEntry _leftClickAudioEntry;
    private AudioEntry _rightClickAudioEntry;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        if (!string.IsNullOrEmpty(HoverAudioId))
            _hoverAudioEntry = HoverAudioId;
        if (!string.IsNullOrEmpty(LeftClickAudioId))
            _leftClickAudioEntry = LeftClickAudioId;
        if (!string.IsNullOrEmpty(RightClickAudioId))
            _rightClickAudioEntry = RightClickAudioId;
        
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            if (_hoverAudioEntry != null)
                _ib.PointerEnterNeuron.Remove(Hover);
            if (_leftClickAudioEntry != null)
                _ib.LeftClickNeuron.Remove(LeftClick);
            if (_rightClickAudioEntry != null)
                _ib.RightClickNeuron.Remove(RightClick);
        }

        _ib = ib;
        if (_ib != null)
        {
            if (_hoverAudioEntry != null)
                _ib.PointerEnterNeuron.Join(Hover);
            if (_leftClickAudioEntry != null)
                _ib.LeftClickNeuron.Join(LeftClick);
            if (_rightClickAudioEntry != null)
                _ib.RightClickNeuron.Join(RightClick);
        }
    }
    
    private void Hover(InteractBehaviour ib, PointerEventData d)
    {
        AudioManager.Play(_hoverAudioEntry);
    }
    
    private void LeftClick(InteractBehaviour ib, PointerEventData d)
    {
        AudioManager.Play(_leftClickAudioEntry);
    }
    
    private void RightClick(InteractBehaviour ib, PointerEventData d)
    {
        AudioManager.Play(_rightClickAudioEntry);
    }
}
