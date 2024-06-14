
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
