
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : Panel
{
    [SerializeField] private Button ImageButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        ImageButton.onClick.RemoveAllListeners();
        ImageButton.onClick.AddListener(ClickedSignal);
    }

    protected override void InitStateMachine()
    {
        SM = new(2);
        // 0 for hide, 1 for show
        SM[0, 1] = ShowTween;
        SM[1, 1] = RefreshTween;
        SM[-1, 0] = HideTween;
        
        SetState(0);
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
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new ClickedSignal());
        PanelS panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
    }
}
