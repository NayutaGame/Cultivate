
using TMPro;
using UnityEngine.UI;

public class NodeView : SimpleView
{
    public Button Button;
    
    public Image Image;
    public TMP_Text NameText;
    public TMP_Text Description;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Click);
    }

    public override void Refresh()
    {
        base.Refresh();
        RunNode runNode = Get<RunNode>();

        Image.sprite = runNode.GetSprite();
        NameText.text = runNode.GetName();
        Description.text = runNode.GetDescription();

        switch (runNode.State)
        {
            case RunNode.RunNodeState.Missed:
            case RunNode.RunNodeState.Passed:
            case RunNode.RunNodeState.Current:
                // _breathingButton.SetColor(Color.black);
                break;
            case RunNode.RunNodeState.ToChoose:
            case RunNode.RunNodeState.Untouched:
                // _breathingButton.SetColor(Color.white);
                break;
        }
    }

    private void Click()
    {
        TryClickNode();
    }

    private bool TryClickNode()
    {
        RunNode runNode = Get<RunNode>();
        if (runNode.State != RunNode.RunNodeState.ToChoose || !RunManager.Instance.Environment.Map.Choosing)
            return false;

        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.MakeChoiceProcedure(runNode);
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        return true;
    }
}
