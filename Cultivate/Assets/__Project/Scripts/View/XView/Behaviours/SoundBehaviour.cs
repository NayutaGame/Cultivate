
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
            _hoverAudioEntry = Encyclopedia.AudioCategory.FromName(HoverAudioId);
        if (!string.IsNullOrEmpty(LeftClickAudioId))
            _leftClickAudioEntry = Encyclopedia.AudioCategory.FromName(LeftClickAudioId);
        if (!string.IsNullOrEmpty(RightClickAudioId))
            _rightClickAudioEntry = Encyclopedia.AudioCategory.FromName(RightClickAudioId);
        
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            if (_hoverAudioEntry != null)
                _ib.NeuronBundle.PointerEnterNeuron.Remove(Hover);
            if (_leftClickAudioEntry != null)
                _ib.NeuronBundle.LeftClickNeuron.Remove(LeftClick);
            if (_rightClickAudioEntry != null)
                _ib.NeuronBundle.RightClickNeuron.Remove(RightClick);
        }

        _ib = ib;
        if (_ib != null)
        {
            if (_hoverAudioEntry != null)
                _ib.NeuronBundle.PointerEnterNeuron.Join(Hover);
            if (_leftClickAudioEntry != null)
                _ib.NeuronBundle.LeftClickNeuron.Join(LeftClick);
            if (_rightClickAudioEntry != null)
                _ib.NeuronBundle.RightClickNeuron.Join(RightClick);
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
