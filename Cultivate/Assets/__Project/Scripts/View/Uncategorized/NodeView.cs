
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeView : ItemView
{
    public JumpingButton _jumpingButton;
    public TMP_Text NameText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        _jumpingButton.RemoveAllListeners();
        _jumpingButton.AddListener(OnPointerClick);
    }

    public override void Refresh()
    {
        base.Refresh();
        RunNode runNode = Get<RunNode>();

        gameObject.SetActive(runNode != null);
        if (runNode == null)
            return;

        if (NameText != null)
            NameText.text = runNode.GetTitle() + runNode.State.ToString();

        _jumpingButton.SetSprite(runNode.Sprite);

        switch (runNode.State)
        {
            case RunNode.RunNodeState.Missed:
            case RunNode.RunNodeState.Passed:
            case RunNode.RunNodeState.Current:
                _jumpingButton.SetColor(Color.black);
                break;
            case RunNode.RunNodeState.ToChoose:
            case RunNode.RunNodeState.Future:
                _jumpingButton.SetColor(Color.white);
                break;
        }

        _jumpingButton.SetJumping(runNode.State == RunNode.RunNodeState.ToChoose);
    }

    private void OnPointerClick(PointerEventData eventData)
    {
        TryClickNode();
        // CanvasManager.Instance.RunCanvas.Refresh();
    }

    private bool TryClickNode()
    {
        RunNode runNode = Get<RunNode>();
        if (runNode.State != RunNode.RunNodeState.ToChoose || !RunManager.Instance.Environment.Map.Selecting)
            return false;

        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.SelectedNode(runNode);
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        return true;
    }
}
