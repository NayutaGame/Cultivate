
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class GlowingButton : MonoBehaviour
{
    [SerializeField] public Button Button;

    [SerializeField] public Image Image;
    
    [SerializeField] public Sprite ActiveSprite;
    [SerializeField] public Sprite InactiveSprite;

    public Neuron OnClickNeuron = new();

    private void OnEnable()
    {
        Button.onClick.AddListener(OnClickNeuron.Invoke);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(OnClickNeuron.Invoke);
    }

    public bool IsInteractable()
        => Button.IsInteractable();
    
    public void SetInteractable(bool interactable)
    {
        Button.interactable = interactable;
        Image.sprite = interactable ? ActiveSprite : InactiveSprite;
    }
}
