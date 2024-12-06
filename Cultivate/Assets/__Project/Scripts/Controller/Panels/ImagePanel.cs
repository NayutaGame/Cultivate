
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : Panel
{
    [SerializeField] private Button ImageButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        ImageButton.onClick.RemoveAllListeners();
        ImageButton.onClick.AddListener(ClickedSignal);
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Image Panel");
        animator[0, 1] = EnterIdle;
        animator[1, 1] = RefreshTween;
        animator[-1, 0] = EnterHide;
        
        animator.SetState(0);
        return animator;
    }

    private Tween RefreshTween()
        => DOTween.Sequence().AppendCallback(Refresh);

    public override void Refresh()
    {
        base.Refresh();
        ImagePanelDescriptor panelDescriptor = _address.Get<ImagePanelDescriptor>();
        ImageButton.image.sprite = panelDescriptor.GetSprite();
    }

    private void ClickedSignal()
    {
        Signal signal = new ClickedSignal();
        CanvasManager.Instance.RunCanvas.LegacySetPanelSAsyncFromSignal(signal);
    }
}
