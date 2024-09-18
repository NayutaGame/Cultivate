
using UnityEngine;
using UnityEngine.UI;

public class ReactionView : MonoBehaviour
{
    [SerializeField] private Image ReactionImage;

    private ReactionState _reactionState;

    public void SetSprite(Sprite sprite)
    {
        ReactionImage.gameObject.SetActive(sprite != null);
        
        if (sprite == null)
            return;

        ReactionImage.sprite = sprite;
    }

    public void SetIntensity(float intensity)
    {
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private AnimationHandle _handle;
}
