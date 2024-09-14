
using TMPro;
using UnityEngine.UI;

public class NodeView : SimpleView
{
    public Image Image;
    public TMP_Text NameText;
    public TMP_Text Description;

    public override void Refresh()
    {
        base.Refresh();
        // RunNode runNode = Get<RunNode>();
        //
        // Image.sprite = runNode.GetSprite();
        // NameText.text = runNode.GetName();
        // Description.text = runNode.GetDescription();

        // switch (runNode.State)
        // {
        //     case RunNode.RunNodeState.Missed:
        //     case RunNode.RunNodeState.Passed:
        //     case RunNode.RunNodeState.Current:
        //         // _breathingButton.SetColor(Color.black);
        //         break;
        //     case RunNode.RunNodeState.ToChoose:
        //     case RunNode.RunNodeState.Untouched:
        //         // _breathingButton.SetColor(Color.white);
        //         break;
        // }
    }

    private bool TryClickNode()
    {
        RunManager.Instance.Environment.Map.NextStep();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.Panel;
        PanelS panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
        return true;
    }
}
