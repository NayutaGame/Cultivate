
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Tween = DG.Tweening.Tween;

public class CombatButton : MonoBehaviour
{
    [SerializeField] public RectTransform _rectTransform;
    [SerializeField] private BreathingButton _breathingButton;

    public Neuron<PointerEventData> ClickNeuron = new();

    private void Start()
    {
        _breathingButton.ClickNeuron.Join(ClickNeuron);
        _breathingButton.SetBreathing(true);
    }

    private Tween _handle;
    public void SetBaseScale(float scale)
    {
        _handle?.Kill();
        _handle = _rectTransform.DOScale(scale, 0.2f).SetAutoKill();
        _handle.Restart();
    }
}
