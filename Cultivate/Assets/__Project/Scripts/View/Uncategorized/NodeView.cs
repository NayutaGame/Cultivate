
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeView : MonoBehaviour, IIndexPath
{
    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public JumpingButton _jumpingButton;
    public TMP_Text NameText;

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;

        _jumpingButton.RemoveAllListeners();
        _jumpingButton.AddListener(OnPointerClick);
    }

    public virtual void Refresh()
    {
        RunNode runNode = DataManager.Get<RunNode>(GetIndexPath());

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
        TryClickNode(GetIndexPath());
        RunCanvas.Instance.Refresh();
    }

    private bool TryClickNode(IndexPath indexPath)
    {
        RunNode runNode = DataManager.Get<RunNode>(indexPath);
        if (runNode.State != RunNode.RunNodeState.ToChoose || !RunManager.Instance.Battle.Map.Selecting)
            return false;

        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.SelectedNode(runNode);
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        return true;
    }
}
